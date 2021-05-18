import { ResponseBody } from '@Palavyr-Types';
import { Response } from 'express';

export const FAIL_TO_STREAM_MESSAGE =
    'Failed write file to stream - check that the pdf extension exists. This is required.';
export const SUCCESS_MESSAGE = 'Succesfully saved pdf to S3';
export const FAILED_TO_UPLOAD_MESSAGE = 'Unable to write pdf file to S3: ';

export const sendResponse = (
    response: Response,
    statusCode: number,
    statuseMessage: string,
    responseBody: ResponseBody | null
): void => {
    response.statusCode = statusCode;
    response.statusMessage = statuseMessage;
    response.send(responseBody);
};

export const createEmptyResponseBody = (): ResponseBody => {
    return {
        S3Key: '',
        FileNameWithExtension: '',
        FileStem: '',
    };
};

export const createSuccessResponseBody = (s3Key: string, identifier: string): ResponseBody => {
    return {
        S3Key: s3Key,
        FileNameWithExtension: identifier + '.pdf',
        FileStem: identifier,
    };
};
