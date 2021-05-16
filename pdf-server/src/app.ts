import express, { Application, Request, Response, NextFunction } from 'express';
import cors from 'cors';
import pdf from 'html-pdf';
import { PutObjectCommandInput, S3Client } from '@aws-sdk/client-s3';
import { getAwsCredentials } from './config/readAwsConfig';
import { Upload } from '@aws-sdk/lib-storage';
import { createEmptyResponseBody, createSuccessResponseBody, FAILED_TO_UPLOAD_MESSAGE, FAIL_TO_STREAM_MESSAGE, sendResponse, SUCCESS_MESSAGE } from 'utils/responses';
import { unpackRequest } from 'utils/request';
import { logDebug } from 'utils/logging';

const app: Application = express();
const port: string = process.env.PORT || '5603'; // Critical This port is hard coded in the API
console.log('starting PDF service on port: ' + port);


// middleware
app.use(cors());
app.use(express.urlencoded({ extended: true }));
app.use(express.json());


///
/// This enpoint is very simple -- it receives a string and writes to a provided path.
/// THE FILENAME MUST BE A VALID PATH AND END IN .pdf or it will not write to disk!
///
app.post('/create-pdf', (req: Request, res: Response, next: NextFunction) => {

	logDebug("RequestReceived: ");
	logDebug(req.body);
	logDebug("------------------------");

	const options = unpackRequest(req);
	pdf.create(options.html, options.paper).toStream(async (err, readStream) => {
		if (err) {
			logDebug(err);
			logDebug(FAIL_TO_STREAM_MESSAGE);
			sendResponse(res, 400, FAIL_TO_STREAM_MESSAGE, createEmptyResponseBody());
		}

		const config = getAwsCredentials();
		logDebug(config);

		const s3 = new S3Client({ region: config.region, credentials: config.credentials });

		const target: PutObjectCommandInput = { Bucket: options.s3bucket, Key: options.s3key, Body: readStream, ContentType: "application/pdf" };
		logDebug(target);

		try {
			logDebug("Attempting to upload the pdf to s3");

			const paralellUploads3 = new Upload({
				client: s3,
				leavePartsOnError: false, // optional manually handle dropped parts
				params: target,
			});

			paralellUploads3.on('httpUploadProgress', (progress) => {
				logDebug(progress);
			});
			logDebug("Making upload call...")
			await paralellUploads3.done();

			logDebug('Saved pdf file to ' + options.s3key);
			const responseBody = createSuccessResponseBody(options.s3key, options.identifier);
			sendResponse(res, 200, SUCCESS_MESSAGE, responseBody);
		} catch (error) {

			logDebug(error);
			logDebug("=====================");
			sendResponse(res, 400, FAILED_TO_UPLOAD_MESSAGE + err, null);
		}
	});
});

app.listen(port, () => console.log('Server listening...'));
