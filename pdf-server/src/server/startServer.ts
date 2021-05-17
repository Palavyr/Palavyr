import { Application } from 'express';
import { logTrace } from 'logging/logging';

export const startServer = (app: Application, port: string) => {
    app.listen(port, () => logTrace('Server listening...'));
};
