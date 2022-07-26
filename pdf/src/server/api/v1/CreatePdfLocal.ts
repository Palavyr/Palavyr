import fs from 'fs';
import { logDebug, logTrace } from 'logging/logging';
import PdfGenerator from 'pdf/pdfGenerator';
import { Application, Request, Response, NextFunction } from 'express';
import { pathToPhantom, pathToScript } from 'utils/pathUtils';
import { ReadStream } from 'fs';
import { FAIL_TO_STREAM_MESSAGE } from 'http/responses/messages';
import responses from 'http/responses/sendResponse';
import { LocalRequestBody } from '@Palavyr-Types';

const createSaveToLocalCallback = async (response: any) => async (filePath: string, stream?: ReadStream) => {
    try {
        // save the read stream to disk here
        if (!stream) {
            responses.createInternalServerErrorResponse(response, null);
            return;
        }

        logTrace(`Attempting to save the pdf to ${filePath}`);
        const writeableStream = fs.createWriteStream(filePath);
        stream.pipe(writeableStream);
        logDebug('Saved to disk');

        responses.createSuccessResponse(response, { filePath });
        logTrace('Saved ' + filePath + ' to local.');
        return;
    } catch (error) {
        logDebug(error);
        responses.createInternalServerErrorResponse(response, null);
        return;
    }
};

const createErrorCallback = (response: any) => async (error: Error | null): Promise<void> => {
    logDebug(error);
    logDebug(FAIL_TO_STREAM_MESSAGE);
    responses.createInternalServerErrorResponse(response, null);
    return;
};


export const create_pdf_on_local_v1 = (app: Application) => {
    app.post('/api/v1/create-pdf-on-local', async (request: Request, response: Response, next: NextFunction) => {
        const { filePath, html, paper } = request.body as LocalRequestBody;
        logTrace(`Local file path: ${filePath}`);
        logTrace(`Html to be converted: ${html}`);
        if (filePath === undefined) {
            responses.createErrorResponse(response, 'File Path was undefined...');
            return;
        }
        if (html === undefined) {
            responses.createErrorResponse(response, 'Html was undefined');
            return;
        }
        if (paper === undefined) {
            responses.createErrorResponse(response, 'Paper options were undefined');
            return;
        }

        const pdf = new PdfGenerator(html, pathToPhantom, pathToScript, paper);
        pdf.toFile(await createSaveToLocalCallback(response), createErrorCallback(response), filePath);
    });
};
