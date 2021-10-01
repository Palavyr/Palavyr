import app from './app';

const handler = require('serverless-express/handler')

module.exports.api = handler(app)