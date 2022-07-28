import app from './src/app';
const handler = require('serverless-express/handler');

const isWindows = process.platform.startsWith('win');
if (!isWindows) {
    // https://github.com/naeemshaikh27/phantom-lambda-fontconfig-pack
    // https://github.com/marcbachmann/node-html-pdf/issues/547
    var path = require('path');

    process.env['FONTCONFIG_PATH'] = path.join(process.env['LAMBDA_TASK_ROOT'], 'fonts');
    path.join(process.env['LAMBDA_TASK_ROOT'], '...');

    process.env.FONTCONFIG_PATH = '/var/task/fonts';
}

exports.handler = handler(app);
