import { PutObjectCommandInput, S3Client } from '@aws-sdk/client-s3';
import { Upload } from '@aws-sdk/lib-storage';
import { RequestBody } from '@Palavyr-Types';
import { Request } from 'express';
import { APPLICATION_PDF } from './contentTypes';

export const unpackRequest = (req: Request): RequestBody => {
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
