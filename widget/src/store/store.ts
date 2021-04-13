import { createStore, combineReducers, compose } from "redux";

import { behaviorReducer } from "./reducers/behaviorReducer";
import { messagesReducer } from "./reducers/messagesReducer";
import { contextReducer } from "./reducers/contextReducer";

declare global {
    interface Window {
        __REDUX_DEVTOOLS_EXTENSION_COMPOSE__?: typeof compose;
    }
}

const composeEnhancers = (process.env.NODE_ENV !== "production" && window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__) || compose;
const reducer = combineReducers({ behaviorReducer, messagesReducer, contextReducer });

export default createStore(reducer, composeEnhancers());
