import cors from 'cors';
import express, { Application } from 'express';

export const configureMiddleware = (app: Application): Application => {
    // middleware
    app.use(cors());
    app.use(express.urlencoded({ extended: true }));
    app.use(express.json());
    return app;
};
