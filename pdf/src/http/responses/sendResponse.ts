import { S3ResponseBody, ResponseMessage, DownloadResponseBody, LocalResponseBody } from '@Palavyr-Types';
import { Response } from 'express';

const sendResponse = (
    response: Response,
    statusCode: number,
    statuseMessage: string,
    responseBody: S3ResponseBody | ResponseMessage | DownloadResponseBody | LocalResponseBody | null
): void => {
    response.statusCode = statusCode;
    response.statusMessage = statuseMessage;
    response.send(responseBody);
};

class ResponseFactory {
    constructor() {}

    public createSuccessResponse(
        response: Response,
        responseBody: S3ResponseBody | ResponseMessage | DownloadResponseBody | LocalResponseBody | null
    ): void {
        sendResponse(response, 200, 'OK', responseBody);
    }

    public createErrorResponse(
        response: Response,
        statusMessage: string,
    ): void {
        sendResponse(response, 400, statusMessage, null);
    }

    public createInternalServerErrorResponse(
        response: Response,
        responseBody: S3ResponseBody | ResponseMessage | DownloadResponseBody | null
    ): void {
        sendResponse(response, 500, 'Internal Server Error', responseBody);
    }

    // create 404 response
    public createNotFoundResponse(response: Response): void {
        sendResponse(response, 404, 'Not Found', null);
    }


}

export default new ResponseFactory();
