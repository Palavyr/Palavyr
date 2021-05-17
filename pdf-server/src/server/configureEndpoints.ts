import { S3Client } from '@aws-sdk/client-s3';
import { configureUpload, createPutRequest, unpackRequest } from 'http/request';
import {
    FAIL_TO_STREAM_MESSAGE,
    sendResponse,
    createEmptyResponseBody,
    SUCCESS_MESSAGE,
    FAILED_TO_UPLOAD_MESSAGE,
    createSuccessResponseBody,
} from 'http/responses';
import { logDebug, logTrace } from 'logging/logging';
import PdfGenerator from 'pdf/pdfGenerator';
import { Application, Request, Response, NextFunction } from 'express';
import { pathToPhantom, pathToScript } from 'utils/pathUtils';

export const configureEndpoints = (app: Application) => {
    ///
    /// This enpoint is very simple -- it receives a string and writes to a provided path.
    /// THE FILENAME MUST BE A VALID PATH AND END IN .pdf or it will not write to disk!
    ///
    app.post('/create-pdf', (request: Request, response: Response, next: NextFunction) => {
        const options = unpackRequest(request);
        const pdf = new PdfGenerator(options.html, pathToPhantom, pathToScript, options.paper);

        pdf.toStream(async (error: any, readStream: any) => {
            if (error) {
                logDebug(error);
                logDebug(FAIL_TO_STREAM_MESSAGE);
                sendResponse(response, 400, FAIL_TO_STREAM_MESSAGE, createEmptyResponseBody());
                return;
            } else {
                const putRequest = createPutRequest(options.bucket, options.key, readStream);
                const parallelUpload = configureUpload(new S3Client(options.s3ClientConfig), putRequest);

                logDebug('Attempting to save to S3...');
                try {
                    await parallelUpload.done();
                    const responseBody = createSuccessResponseBody(options.key, options.identifier);
                    sendResponse(response, 200, SUCCESS_MESSAGE, responseBody);
                    logTrace('Saved ' + options.key + ' to S3.');
                    return;
                } catch (error) {
                    logDebug(error);
                    sendResponse(response, 400, FAILED_TO_UPLOAD_MESSAGE + error, null);
                    return;
                }
            }
        });
    });
    return app;
};
