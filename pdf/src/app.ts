import { port } from 'http/port';
import { configureEndpoints } from 'server/configureEndpoints';
import { createServer } from 'server/createServer';

const server = createServer(port);
configureEndpoints(server);

export default server;
