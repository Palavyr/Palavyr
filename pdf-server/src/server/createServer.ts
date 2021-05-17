import express, { Application } from 'express';
import { configureMiddleware } from 'http/middleware';
import { logTrace } from 'logging/logging';

export const createServer = (port: string) => {
    const app: Application = express();
    configureMiddleware(app);
    logTrace('starting PDF service on port: ' + port);
    return app;
};
