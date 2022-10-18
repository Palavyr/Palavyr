import { S3ClientConfig } from '@aws-sdk/client-s3';
import { ReadStream } from 'fs';

export type SnackbarPositions = 'tr' | 't' | 'tl' | 'bl' | 'b' | 'br';

export type PdfErrorCallback = (error: Error | null) => void;
export type ResponseMessage = string;

export type DownloadRequestBody = {
    html: string;
    paper: CreateOptions;
    identifier: string;
};

export type DownloadResponseBody = {
    stream: ReadStream | null;
};

export type LocalRequestBody = {
    filePath: string;
    html: string;
    paper: CreateOptions;
};

export type LocalResponseBody = {
    filePath: string;
};

export type S3RequestBody = {
    bucket: string;
    key: string;
    html: string;
    identifier: string;
    region: string;
    accesskey: string;
    secretkey: string;
    paper: CreateOptions;
    endpoint: string;
};

export type S3ResponseBody = {
    s3Key: string;
    fileNameWithExtension: string;
    fileStem: string;
};

export interface CreateOptions {
    // Export options
    directory?: string;
    filename?: string;
    localUrlAccess: boolean;

    // Papersize Options: http://phantomjs.org/api/webpage/property/paper-size.html
    height?: string;
    width?: string;
    format?: 'A3' | 'A4' | 'A5' | 'Legal' | 'Letter' | 'Tabloid';
    orientation?: 'portrait' | 'landscape';

    // Page options
    border?:
        | string
        | {
              top?: string;
              right?: string;
              bottom?: string;
              left?: string;
          };

    paginationOffset?: number;

    header?: {
        height?: string;
        contents?: string;
    };
    footer?: {
        height?: string;
        contents?: {
            first?: string;
            [page: number]: string;
            default?: string;
            last?: string;
        };
    };

    // Rendering options
    base?: string;

    // Zooming option, can be used to scale images if `options.type` is not pdf
    zoomFactor?: string;

    // File options
    type?: 'png' | 'jpeg' | 'pdf';
    quality?: string;

    // Script options
    phantomPath?: string;
    phantomArgs?: string[];
    script?: string;
    timeout?: number;

    // Time we should wait after window load
    renderDelay?: 'manual' | number;

    // HTTP Headers that are used for requests
    httpHeaders?: {
        [header: string]: string;
    };

    // To run Node application as Windows service
    childProcessOptions?: {
        detached?: boolean;
    };

    // HTTP Cookies that are used for requests
    httpCookies?: Array<{
        name: string;
        value: string;
        domain?: string;
        path: string;
        httponly?: boolean;
        secure?: boolean;
        expires?: number;
    }>;
}

export interface FileInfo {
    filename: string;
}

export interface IPdfGenerator {
    // toBuffer(callback: (err: Error, buffer: Buffer) => void): void;
    toStream(successCallback: (stream?: ReadStream) => void, errorCallback: (error: Error | null) => void): void;
    toFile(
        toFileCallback: (filePath: string, stream: ReadStream) => void,
        errorCallback: (error: Error | null) => void,
        filePath: string
    ): void;
}
