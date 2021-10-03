// import * as serverlessExpress from 'aws-serverless-express';
// import app from './src/app';

// const server = serverlessExpress.createServer(app);

// exports.handlerMethod = async (event: any, context: any) => {
//     serverlessExpress.proxy(server, event, context);
// };


import serverlessExpress from '@vendia/serverless-express'
import app from './src/app'
exports.handler = serverlessExpress({ app })