import { Application, Request, Response, NextFunction } from 'express';
import { S3RequestBody } from '@Palavyr-Types';
import { logTrace } from 'logging/logging';
import pdf from 'html-pdf';
import responses from 'http/responses/sendResponse';
import aws from 'aws-sdk';
import { ReadStream } from 'fs';
import { APPLICATION_PDF } from 'http/contentTypes';

export const WriteToS3 = (html: string, options: S3RequestBody, response: Response) => {
  const s3 = new aws.S3(options.s3ClientConfig as aws.S3.ClientConfiguration);

  pdf.create(html).toStream(function(err: Error, stream: ReadStream) {
    if (err) {
      logTrace('Critical Error');
      logTrace(err);
      responses.createInternalServerErrorResponse(response, null);
    }

    logTrace('Created pdf stream...');
    const params: aws.S3.PutObjectRequest = {
      Key: options.key,
      Body: stream,
      Bucket: options.bucket,
      ContentType: APPLICATION_PDF,
    };

    logTrace('Attempting to upload to s3, using '+ options.key + ' and ' + options.bucket)
    s3.upload(params, function(err: any, res: any) {
      if (err) {
        logTrace('ERROR: ' + err);
        responses.createInternalServerErrorResponse(response, null);
      } else {
        logTrace('Uploaded...');
        responses.createSuccessResponse(response, {
          s3Key: options.key,
          fileNameWithExtension: options.identifier + '.pdf',
          fileStem: options.identifier,
        });
      }
    });
  });
};

const unpackS3Request = (req: Request): S3RequestBody => {
  return {
    bucket: req.body.Bucket,
    key: req.body.Key,
    html: req.body.Html,
    identifier: req.body.Id,
    s3ClientConfig: {
      forcePathStyle: true,
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
    WriteToS3(options.html, options, response);
  });
};
