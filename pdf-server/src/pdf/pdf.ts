/* tslint:disable */

import { readFile, unlink, createReadStream } from 'fs';
import { spawn } from 'child_process';
import { normalize, join, resolve } from 'path';
import assert from 'assert';
import { CreateResult, FileInfo } from '@Palavyr-Types';



try {
	var phantomjs = require('phantomjs-prebuilt');
} catch (err) {
	console.log('html-pdf: Failed to load PhantomJS module.', err);
}

/*
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


export type PdfOptions = {
	script: string;
	filename: string;
	phantomPath: string;
	phantomArgs: string[];
	localUrlAccess: string;
	timeout: number;
	childProcessOptions: any;
}



class PDF {

	public html: string;
	public script: string;
	public options: PdfOptions;

	constructor(html: string, options?: any) {
		this.html = html;
		this.options = options || {};
		if (this.options.script) {
			this.script = normalize(this.options.script);
		} else {
			this.script = join(__dirname,  './pdf_a4_portrait.js')
		}

		if (this.options.filename) this.options.filename = resolve(this.options.filename);
		if (!this.options.phantomPath) this.options.phantomPath = phantomjs && phantomjs.path;
		this.options.phantomArgs = this.options.phantomArgs || [];

		if (!this.options.localUrlAccess) this.options.phantomArgs.push('--local-url-access=false');
		assert(
			this.options.phantomPath,
			'html-pdf: Failed to load PhantomJS module. You have to set the path to the PhantomJS binary using options.phantomPath'
		);
		assert(
			typeof this.html === 'string' && this.html.length,
			'html-pdf: Cant create a pdf without an html string'
		);

		this.options.timeout = this.options.timeout || 30000;

	}
	// toBuffer(callback: any) {
	// 	this.exec(function execPdfToBuffer(err: any, res: FileInfo) {
	// 		if (err) return callback(err);
	// 		readFile(res.filename, function readCallback(err, buffer) {
	// 			if (err) return callback(err);
	// 			unlink(res.filename, function unlinkPdfFile(err) {
	// 				if (err) return callback(err);
	// 				callback(null, buffer);
	// 			});
	// 		});
	// 	});
	// }
	toStream(callback: any) {
		this.exec(function(err: any, res: FileInfo) {
			if (err) return callback(err);
			try {
				var stream = createReadStream(res.filename);
			} catch (err) {
				return callback(err);
			}

			stream.on('end', function() {
				unlink(res.filename, function unlinkPdfFile(err) {
					if (err) console.log('html-pdf:', err);
				});
			});

			callback(null, stream);
		});
	}
	// toFile(filename?: string, callback) {
	// 	assert(arguments.length > 0, 'html-pdf: The method .toFile([filename, ]callback) requires a callback.');
	// 	if (filename instanceof Function) {
	// 		callback = filename;
	// 		filename = undefined;
	// 	} else {
	// 		this.options.filename = resolve(filename);
	// 	}
	// 	this.exec(callback);
	// }
	exec(callback: any) {

		const child = spawn(
			"C:\\Users\\paule\\Desktop\\phantomjs-2.1.1-windows\\phantomjs-2.1.1-windows\\bin\\phantomjs.exe",
			[...this.options.phantomArgs, "C:\\Users\\paule\\code\\palavyr\\Configuration-Manager\\pdf-server\\src\\pdf\\pdf_a4_portrait.js"],
			this.options.childProcessOptions
		);
		const stderr: any = [];

		const timeout = setTimeout(() => {
			respond(null, new Error('html-pdf: PDF generation timeout. Phantom.js script did not exit.'), null);
		}, this.options.timeout);

		function onError(buffer: any) {
			stderr.push(buffer);
		}

		function onData(buffer: any) {
			let result;
			try {
				const json = buffer.toString().trim();
				if (json) result = JSON.parse(json);
			} catch (err) {
				// Proxy for debugging purposes
				process.stdout.write(buffer);
			}

			if (result) respond(null, null, result);
		}

		let callbacked = false;
		function respond(code: any, err:any, data: any) {
			if (callbacked) return;
			callbacked = true;
			clearTimeout(timeout);

			// If we don't have an exit code, we kill the process, ignore stderr after this point
			if (code === null) kill(child, onData, onError);

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
		}

		child.stdout.on('data', onData);
		child.stderr.on('data', onError);
		child.on('error', function onError(err) {
			respond(null, err, null);
		});

		// An exit event is most likely an error because we didn't get any data at this point
		child.on('close', respond);
		child.on('exit', respond);

		const config = JSON.stringify({ html: this.html, options: this.options });
		child.stdin.write(config + '\n', 'utf8');
		child.stdin.end();
	}
}

export const kill = (child: any, onData: any, onError: any) => {
	child.stdin.end();
	child.kill();
};

export default PDF;
