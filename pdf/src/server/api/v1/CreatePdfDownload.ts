import { logDebug, logTrace } from 'logging/logging';
import PdfGenerator from 'pdf/pdfGenerator';
import { Application, Request, Response, NextFunction } from 'express';
import { pathToPhantom, pathToScript } from 'utils/pathUtils';
import { ReadStream } from 'fs';
import { FAIL_TO_STREAM_MESSAGE } from 'http/responses/messages';
import responses from 'http/responses/sendResponse';
import { DownloadRequestBody } from '@Palavyr-Types';
import fs from 'fs';
import os from 'os';
import path from 'path';
// import puppeteer from 'puppeteer-serverless';

const PALAVYR_TEMP_DIRECTORY = 'palavyr-pdf-server-temp';

const createDownload = async (response: Response, identifier: string) => async (stream?: ReadStream) => {
    if (!stream) {
        logDebug(FAIL_TO_STREAM_MESSAGE);
        responses.createErrorResponse(response, FAIL_TO_STREAM_MESSAGE);
        return;
    }

    const tempDirectory = path.join(os.tmpdir(), PALAVYR_TEMP_DIRECTORY);
    if (!fs.existsSync(tempDirectory)) {
        fs.mkdirSync(tempDirectory);
    }

    const fileName = `${identifier}.pdf`;
    const filePath = path.join(tempDirectory, fileName);
    logDebug(`Saving file to ${filePath}`);

    const writeableStream = fs.createWriteStream(filePath);
    stream.pipe(writeableStream);

    responses.createSuccessResponse(response, null);
};

const createErrorCallback = (response: any) => async (error: Error | null): Promise<void> => {
    responses.createErrorResponse(response, FAIL_TO_STREAM_MESSAGE);
    return;
};

export const create_pdf_download_v1 = (app: Application) => {
    // app.post('/api/v1/create-pdf-download', async (request: Request, response: Response, next: NextFunction) => {
    //     const { html, paper, identifier } = request.body as DownloadRequestBody;
    //     logTrace(html);
    //     const pdf = new PdfGenerator(html, pathToPhantom, pathToScript, paper);
    //     pdf.toStream(await createDownload(response, identifier), createErrorCallback(response));
    // });

    // app.get('/api/v1/download-pdf/:fileId', async (request: Request, response: Response, next: NextFunction) => {
    //     const fileId = request.params.fileId;
    //     logTrace('Server again received ID: ' + fileId);
    //     if (fileId === undefined) {
    //         responses.createErrorResponse(response, 'No file id provided');
    //         return;
    //     }
    //     const filePath = path.join(os.tmpdir(), PALAVYR_TEMP_DIRECTORY, fileId + '.pdf');

    //     if (!fs.existsSync(filePath)) {
    //         responses.createErrorResponse(response, 'File does not exist');
    //     }

    //     logTrace(`Sending file ${filePath}`);
    //     logTrace('FINAL CHECK ON FILEID: ' + fileId);
    //     response.download(filePath, `${fileId}.pdf`, (error: Error) => {
    //         if (error) {
    //             responses.createErrorResponse(response, 'Failed to download file');
    //             fs.unlinkSync(filePath);
    //         } else {
    //             fs.unlinkSync(filePath);
    //         }
    //     });
    // });
};

export const create_pupetteer_pdf_download_v1 = (app: Application): void => {
    // app.post(
    //     '/api/v1/create-pdf-download-puppeteer',
    //     async (request: Request, response: Response, next: NextFunction) => {
    //         let browser = null;
    //         let pdf = null;

    //         try {
    //             browser = await puppeteer.launch({});
    //             const page = await browser.newPage();
    //             await page.setContent('<html><body><p>Test</p></body></html>', {
    //                 waitUntil: 'load',
    //             });

    //             pdf = await page.pdf({
    //                 format: 'A4',
    //                 printBackground: true,
    //                 displayHeaderFooter: true,
    //                 margin: {
    //                     top: 40,
    //                     right: 0,
    //                     bottom: 40,
    //                     left: 0,
    //                 },
    //                 headerTemplate: `
    //           <div style="border-bottom: solid 1px gray; width: 100%; font-size: 11px;
    //                 padding: 5px 5px 0; color: gray; position: relative;">
    //           </div>`,
    //                 footerTemplate: `
    //           <div style="border-top: solid 1px gray; width: 100%; font-size: 11px;
    //               padding: 5px 5px 0; color: gray; position: relative;">
    //               <div style="position: absolute; right: 20px; top: 2px;">
    //                 <span class="pageNumber"></span>/<span class="totalPages"></span>
    //               </div>
    //           </div>
    //         `,
    //             });
    //         } finally {
    //             if (browser !== null) {
    //                 await browser.close();
    //             }
    //         }

    //         const payload = {
    //             headers: {
    //                 'Content-type': 'application/pdf',
    //                 'content-disposition': 'attachment; filename=test.pdf',
    //             },
    //             statusCode: 200,
    //             body: pdf.toString('base64'),
    //             isBase64Encoded: true,
    //         };

    //         return payload;

    //         // return {
    //         //     headers: {
    //         //         'Content-type': 'application/pdf',
    //         //         'content-disposition': 'attachment; filename=test.pdf',
    //         //     },
    //         //     statusCode: 200,
    //         //     body: pdf.toString('base64'),
    //         //     isBase64Encoded: true,
    //         // };
    //     }
    // );
};
