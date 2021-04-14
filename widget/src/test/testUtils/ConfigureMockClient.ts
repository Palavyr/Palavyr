import axios from "axios";
import MockAdapter from "axios-mock-adapter";

export class ConfigureMockClient {
    public mock: MockAdapter;
    constructor() {
        this.mock = new MockAdapter(axios);
    }

    public ConfigureGet(routeUrl: string, responseData?: any, statusCode: number = 200) {
        this.mock.onGet(routeUrl).reply(statusCode, responseData)
    }
}