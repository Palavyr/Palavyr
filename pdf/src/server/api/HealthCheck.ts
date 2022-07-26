import { logTrace } from 'logging/logging';
import { Application, Request, Response, NextFunction } from 'express';
import responses from 'http/responses/sendResponse';

export const health_check = (app: Application) => {
    app.get('/health-check', (request: Request, response: Response, next: NextFunction) => {
        logTrace('Very Healthy!');
        responses.createSuccessResponse(response, null);
    });
};
