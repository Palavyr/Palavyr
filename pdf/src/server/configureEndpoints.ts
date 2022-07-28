import { Application } from 'express';
import { create_pdf_on_local_v1 } from './api/v1/CreatePdfLocal';
import { create_pdf_download_v1 } from './api/v1/CreatePdfDownload';
import { create_pdf_on_s3_v1 } from './api/v1/CreatePdfOnS3';
import { health_check } from './api/HealthCheck';

export const configureEndpoints = (app: Application) => {

    // V1 API
    create_pdf_on_s3_v1(app);
    // create_pdf_on_local_v1(app);
    // create_pdf_download_v1(app);

    health_check(app);
    return app;
};
