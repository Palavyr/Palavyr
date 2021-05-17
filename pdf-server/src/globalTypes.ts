import { S3ClientConfig } from '@aws-sdk/client-s3';
import * as fs from 'fs';

export type ResponseBody = {
    FullPath: string;
    FileNameWithExtension: string;
    FileStem: string;
};

export type RequestBody = {
    bucket: string;
    key: string;
    html: string;
    identifier: string;
    s3ClientConfig: S3ClientConfig;
    paper: CreateOptions;
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

export interface CreateResult {
    // toBuffer(callback: (err: Error, buffer: Buffer) => void): void;
    // toFile(callback: (err: Error, res: FileInfo) => void): void;
    // toFile(filename?: string, callback?: (err: Error, res: FileInfo) => void): void;
    toStream(callback: (err: Error, stream: fs.ReadStream) => void): void;
}
