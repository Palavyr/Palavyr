export const port = process.env.PORT as string || '5603';

if (port === undefined) {
    throw new Error('Port is not specified');
}
