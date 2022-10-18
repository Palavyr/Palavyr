import { Application, Request, Response, NextFunction } from 'express';
import { S3RequestBody } from '@Palavyr-Types';
import { logTrace } from 'logging/logging';
import pdf from 'html-pdf';
import responses from 'http/responses/sendResponse';
import AWS from 'aws-sdk';
import { ReadStream } from 'fs';
import { APPLICATION_PDF } from 'http/contentTypes';

export const create_pdf_on_s3_v1 = (app: Application) => {
  app.post('/api/v1/create-pdf-on-s3', async (request: Request, response: Response, next: NextFunction) => {

    logTrace('Received request to create PDF on S3.');

    const options = unpackS3Request(request);

    WriteToS3(options.html, options, response);
  });
};


export const WriteToS3 = (html: string, options: S3RequestBody, response: Response) => {

  const credentials = {
    accessKeyId: options.accesskey,
    secretAccessKey: options.secretkey,
  }

  const S3 = new AWS.S3({
    credentials,
    endpoint: options.endpoint,
    s3ForcePathStyle: true
  });

  pdf.create(html).toStream(function (err: Error, stream: ReadStream) {
    if (err) {
      logTrace('Critical Error');
      logTrace(err);
      responses.createInternalServerErrorResponse(response, null);
    }

    logTrace('Created pdf stream...');
    const params: AWS.S3.PutObjectRequest = {
      Key: options.key,
      Body: stream,
      Bucket: options.bucket,
      ContentType: APPLICATION_PDF,
    };

    logTrace('Attempting to upload to s3, using ' + options.key + ' and ' + options.bucket)
    S3.upload(params, function (err: any, res: any) {
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

  const bucket = req.body.bucket;
  logTrace("Bucket: " + bucket)

  const key = req.body.key;
  logTrace("key: " + key)

  const html = req.body.html;
  logTrace("html: " + html)

  const identifier = req.body.identifier;
  logTrace("identifier: " + identifier)

  const paper = req.body.Paper;
  logTrace("paper: " + paper)

  const accesskey = req.body.accesskey;
  logTrace("accesskey: " + accesskey)

  const secretkey = req.body.secretkey;
  logTrace("secretkey: " + secretkey)

  const region = req.body.region;
  logTrace("region: " + region)

  const endpoint = req.body.endpoint;
  logTrace("Endpoint: " + endpoint)

  return {
    bucket,
    key,
    html,
    identifier,
    paper,
    accesskey,
    secretkey,
    region,
    endpoint
  };
};
