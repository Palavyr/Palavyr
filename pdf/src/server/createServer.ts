import { Express } from 'express';
const express = require('serverless-express/express');
import { configureMiddleware } from 'http/middleware';
import { logTrace } from 'logging/logging';

export const createServer = (port: string) => {
    const app: Express = express();
    configureMiddleware(app);
    logTrace('starting PDF service on port: ' + port);
    return app;
};