import express, { Application, Request, Response, NextFunction } from 'express';
import cors from 'cors';

import { PutObjectCommandInput, S3Client } from '@aws-sdk/client-s3';
import { getAwsCredentials } from './config/readAwsConfig';
import { Upload } from '@aws-sdk/lib-storage';
import {
	createEmptyResponseBody,
	createSuccessResponseBody,
	FAILED_TO_UPLOAD_MESSAGE,
	FAIL_TO_STREAM_MESSAGE,
	sendResponse,
	SUCCESS_MESSAGE
} from 'utils/responses';
import { unpackRequest } from 'utils/request';
import { logDebug } from 'utils/logging';
import path from 'path';
import PDF from 'pdf/pdf';

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

	const options = unpackRequest(req);

	const opts = {
		...options.paper,
		// phantomPath: 'C:\\Users\\paule\\Desktop\\phantomjs-2.1.1-windows\\phantomjs-2.1.1-windows\\bin\\phantomjs.exe'
	};
	const pdf = new PDF(options.html, opts);

	pdf.toStream(async (err: any, readStream: any) => {
		if (err) {
			logDebug(err);
			logDebug(FAIL_TO_STREAM_MESSAGE);
			// throw new Error(err);
			sendResponse(res, 400, FAIL_TO_STREAM_MESSAGE, createEmptyResponseBody());
		}

		const config = getAwsCredentials();
		logDebug(config);

		const s3 = new S3Client({ region: config.region, credentials: config.credentials });

		const target: PutObjectCommandInput = {
			Bucket: options.s3bucket,
			Key: options.s3key,
			Body: readStream,
			ContentType: 'application/pdf'
		};
		logDebug(target);

		try {
			logDebug('Attempting to upload the pdf to s3');
			// console.log(readStream)
			const paralellUploads3 = new Upload({
				client: s3,
				leavePartsOnError: false, // optional manually handle dropped parts
				params: target
			});

			paralellUploads3.on('httpUploadProgress', (progress) => {
				logDebug(progress);
			});
			logDebug('Making upload call...');
			await paralellUploads3.done();

			logDebug('Saved pdf file to ' + options.s3key);
			// const responseBody = createSuccessResponseBody(options.s3key, options.identifier);

			const responseBody = {
				FullPath: options.s3key,
				FileNameWithExtension: options.identifier + '.pdf',
				FileStem: options.identifier,
				TempDirectory: options.s3key.split('/').slice(0, -1).join('/')
			};
			res.statusCode = 200;
			res.statusMessage = SUCCESS_MESSAGE;
			res.send(responseBody);

			// sendResponse(res, 200, SUCCESS_MESSAGE, responseBody);
		} catch (error) {
			logDebug(error);
			logDebug('=====================');
			sendResponse(res, 400, FAILED_TO_UPLOAD_MESSAGE + err, null);
		}
	});
});

app.listen(port, () => console.log('Server listening...'));
