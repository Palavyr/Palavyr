import { PutObjectCommandInput, S3Client } from '@aws-sdk/client-s3';
import { logDebug, logTrace } from 'logging/logging';
import PdfGenerator from 'pdf/pdfGenerator';
import { Application, Request, Response, NextFunction } from 'express';
import { pathToPhantom, pathToScript } from 'utils/pathUtils';
import { ReadStream } from 'fs';
import { FAIL_TO_STREAM_MESSAGE, SUFFIX_WAS_NOT_PDF } from 'http/responses/messages';
import { S3RequestBody } from '@Palavyr-Types';
import responses from 'http/responses/sendResponse';
import { APPLICATION_PDF } from 'http/contentTypes';
import { Upload } from '@aws-sdk/lib-storage';

export const createPutRequest = (bucket: string, key: string, stream: any): PutObjectCommandInput => {
    return {
        Bucket: bucket,
        Key: key,
        Body: stream,
        ContentType: APPLICATION_PDF,
    };
};

export const configureUpload = (client: S3Client, target: PutObjectCommandInput): Upload => {
    return new Upload({
        client,
        leavePartsOnError: false, // optional manually handle dropped parts
        params: target,
    });
};

const createSaveToS3Callback = async (response: any, options: S3RequestBody) => {
    return async (readStream?: ReadStream) => {
        if (!options.key.endsWith('.pdf')) {
            responses.createErrorResponse(response, SUFFIX_WAS_NOT_PDF);
            return;
        }

        try {
            logDebug('Attempting to save to S3...');
            const putRequest = createPutRequest(options.bucket, options.key, readStream);
            const parallelUpload = configureUpload(new S3Client(options.s3ClientConfig), putRequest);
            await parallelUpload.done();
        } catch (error) {
            logDebug(error);
            responses.createInternalServerErrorResponse(response, null);
            return;
        }

        logTrace('Saved ' + options.key + ' to S3.');
        responses.createSuccessResponse(response, {
            s3Key: options.key,
            fileNameWithExtension: options.identifier + '.pdf',
            fileStem: options.identifier,
        });
    };
};

const createErrorCallback = (response: any) => async (error: Error | null): Promise<void> => {
    logDebug(error);
    logDebug(FAIL_TO_STREAM_MESSAGE);
    responses.createErrorResponse(response, FAIL_TO_STREAM_MESSAGE);
    return;
};

const unpackS3Request = (req: Request): S3RequestBody => {
    return {
        bucket: req.body.Bucket,
        key: req.body.Key,
        html: req.body.Html,
        identifier: req.body.Id,
        s3ClientConfig: {
            region: req.body.Region,
            credentials: {
                secretAccessKey: req.body.SecretKey,
                accessKeyId: req.body.AccessKey,
            },
        },
        paper: req.body.Paper, // TODO: provide defaults
    };
};

export const create_pdf_on_s3_v1 = (app: Application) => {
    app.post('/api/v1/create-pdf-on-s3', async (request: Request, response: Response, next: NextFunction) => {
        const options = unpackS3Request(request);
        const pdf = new PdfGenerator(options.html, pathToPhantom, pathToScript, options.paper);
        pdf.toStream(await createSaveToS3Callback(response, options), createErrorCallback(response));
    });
};
