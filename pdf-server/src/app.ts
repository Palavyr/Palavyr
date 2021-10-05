import { port } from './http/port';
import { configureEndpoints } from './server/configureEndpoints';
import { createServer } from './server/createServer';

const app = createServer(port);
configureEndpoints(app);

export default app;