import { PutObjectCommandInput, S3Client } from '@aws-sdk/client-s3';
import PdfGenerator from 'pdf/pdfGenerator';
import { Application, Request, Response, NextFunction } from 'express';
import { pathToPhantom, pathToScript } from 'utils/pathUtils';
import { ReadStream } from 'fs';
import { FAIL_TO_STREAM_MESSAGE, SUFFIX_WAS_NOT_PDF } from 'http/responses/messages';
import { S3RequestBody } from '@Palavyr-Types';
import responses from 'http/responses/sendResponse';
import { APPLICATION_PDF } from 'http/contentTypes';
import { Upload } from '@aws-sdk/lib-storage';
import { logTrace } from 'logging/logging';

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

const createSaveToS3Callback = (response: any, options: S3RequestBody) => {
  logTrace('creating save to S3 callback');
  return async (readStream?: ReadStream) => {
    logTrace('Inside the s3 callback');

    if (!options.key.endsWith('.pdf')) {
      responses.createErrorResponse(response, SUFFIX_WAS_NOT_PDF);
      return;
    }

    try {
      logTrace('Attempting to save to S3...');
      const putRequest = createPutRequest(options.bucket, options.key, readStream);
      const parallelUpload = configureUpload(new S3Client(options.s3ClientConfig), putRequest);
      await parallelUpload.done();
    } catch (error) {
      logTrace(error);
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
  logTrace(error);
  logTrace(FAIL_TO_STREAM_MESSAGE);
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
    logTrace('Received request to create PDF on S3.');

    const options = unpackS3Request(request);
    logTrace(options);
    const pdf = new PdfGenerator(options.html, pathToPhantom, pathToScript, options.paper);
    try {
      logTrace('TRYING TO DO THE THING');
      pdf.toStream(createSaveToS3Callback(response, options), createErrorCallback(response));
    } catch (err) {
      logTrace(err);
      next(err);
    }
  });
};
