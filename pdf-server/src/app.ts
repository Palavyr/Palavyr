import { port } from 'http/port';
import { configureEndpoints } from 'server/configureEndpoints';
import { createServer } from 'server/createServer';
import { startServer } from 'server/startServer';

const app = createServer(port);
configureEndpoints(app);
startServer(app, port);
