import { unlink, createReadStream, ReadStream } from 'fs';
import { spawn } from 'child_process';
import assert from 'assert';
import { FileInfo } from '@Palavyr-Types';
import { logDebug } from 'logging/logging';

/* This code is 'borrowed' and refactored from https://github.com/marcbachmann/node-html-pdf
 * phantomjs version 1.8.1 and later should work.
 *
 * Create a PDF file out of an html string.
 *
 * Regions for the PDF page are:
 *
 * - Page Header  -> document.getElementById('pageHeader')
 * - Page Content -> document.getElementById('pageContent')
 * - Page Footer  -> document.getElementById('pageFooter')
 *
 * When no #pageContent is available, phantomjs will use document.body as pdf content
 */

class PdfGenerator {
    public html: string;
    public pathToPhantom: string;
    public pathToScript: string;
    public paperOptions: any;

    public timeout: number;
    public phantomArgs: string[];
    public filename: string;

    constructor(html: string, pathToPhantom: string, pathToScript: string, paperOptions: any = {}) {
        this.html = html;
        this.paperOptions = paperOptions;
        this.pathToPhantom = pathToPhantom;
        this.pathToScript = pathToScript;
        this.filename = 'temp';
        this.phantomArgs = ['--local-url-access=false'];
        this.timeout = 30000;

        logDebug('Setting Filename as ' + this.filename);
        logDebug('pathToScript: ' + this.pathToScript);
        logDebug('Using phantom options: ' + this.phantomArgs);

        assert(
            this.pathToPhantom,
            'html-pdf: Failed to load PhantomJS module. You have to set the path to the PhantomJS binary'
        );
        assert(typeof this.html === 'string' && this.html.length, 'html-pdf: Cant create a pdf without an html string');
    }

    toStream(callback: (error: Error | null, stream?: ReadStream) => void) {
        logDebug('toStream called...');
        this.exec((error: Error | null, res?: FileInfo) => {
            if (error) {
                logDebug('toStream execution error. Calling the callback with an error');
                return callback(error);
            }
            try {
                logDebug('Attempting to create the read stream');
                var stream = createReadStream(res!.filename);
            } catch (error) {
                return callback(error);
            }

            logDebug('Setting stream Success function');
            stream.on('end', function () {
                unlink(res!.filename, function unlinkPdfFile(error) {
                    if (error) console.log('html-pdf:', error);
                });
            });

            logDebug('Executing tostream callback (Save to S3)');
            callback(null, stream);
        });
    }

    exec(callback: (error: Error | null, res?: FileInfo) => void) {
        logDebug('Execution method inside toStream called...');
        const child = spawn(this.pathToPhantom, [...this.phantomArgs, this.pathToScript]);
        logDebug('Child process spawned...');

        const stderr: Uint8Array[] = [];

        const timeout = setTimeout(() => {
            respond(null, new Error('html-pdf: PDF generation timeout. Phantom.js script did not exit.'), null);
        }, this.timeout);

        const onError = (buffer: any) => {
            stderr.push(buffer);
        };

        const onData = (buffer: any) => {
            let result;
            try {
                const json = buffer.toString().trim();
                if (json) result = JSON.parse(json);
            } catch (err) {
                // Proxy for debugging purposes
                process.stdout.write(buffer);
            }

            if (result) respond(null, null, result);
        };

        let callbacked = false;
        const respond = (code: any, err: any, data: any) => {
            if (callbacked) return;
            callbacked = true;
            clearTimeout(timeout);

            // If we don't have an exit code, we kill the process, ignore stderr after this point
            if (code === null) {
                kill(child, onData, onError);
            }

            // Since code has a truthy/falsy value of either 0 or 1, check for existence first.
            // Ignore if code has a value of 0 since that means PhantomJS has executed and exited successfully.
            // Also, as per your script and standards, having a code value of 1 means one can always assume that
            // an error occured.
            if ((typeof code !== 'undefined' && code !== null && code !== 0) || err) {
                let error = null;

                if (err) {
                    // Rudimentary checking if err is an instance of the Error class
                    error = err instanceof Error ? err : new Error(err);
                } else {
                    // This is to catch the edge case of having a exit code value of 1 but having no error
                    error = new Error('html-pdf: Unknown Error');
                }

                // Append anything caught from the stderr
                const postfix = stderr.length ? '\n' + Buffer.concat(stderr).toString() : '';
                if (postfix) error.message += postfix;

                return callback(error);
            }

            callback(null, data);
        };

        child.stdout.on('data', onData);
        child.stderr.on('data', onError);
        child.on('error', function onError(err) {
            respond(null, err, null);
        });

        // An exit event is most likely an error because we didn't get any data at this point
        child.on('close', respond);
        child.on('exit', respond);

        logDebug('Sending script parameters via standard in -- options used: ' + this.paperOptions);
        const config = JSON.stringify({ html: this.html, options: this.paperOptions });
        child.stdin.write(config + '\n', 'utf8');
        child.stdin.end();
    }
}

export const kill = (child: any, onData: any, onError: any) => {
    child.stdin.end();
    child.kill();
};

export default PdfGenerator;
