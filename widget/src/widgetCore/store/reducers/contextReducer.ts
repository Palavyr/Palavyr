import { ContextPropertyActions, SET_CONTEXT_PROPERTIES, SET_DYNAMICRESPONSE_CONTEXT, SET_EMAILADDRESS_CONTEXT, SET_KEYVALUE_CONTEXT, SET_NAME_CONTEXT, SET_PHONE_CONTEXT, SET_REGION_CONTEXT } from "../actions/types";
import { ContextProperties, ContextState } from "../types";

export const defaultContextProperties: ContextProperties = {
    dynamicResponses: [],
    keyValues: [],
    emailAddress: "",
    phoneNumber: "",
    name: "",
    region: "",
};


const contextReducer = {
    [SET_CONTEXT_PROPERTIES]: (state: ContextState, {updatedContextProperties}): ContextState => ({ ...state, ...{ contextProperties: updatedContextProperties } }),
    [SET_NAME_CONTEXT]: (state: ContextState, {name}): ContextState => ({ ...state, name: name }),
    [SET_PHONE_CONTEXT]: (state: ContextState, {phoneNumber}): ContextState => ({ ...state, phoneNumber: phoneNumber }),
    [SET_EMAILADDRESS_CONTEXT]: (state: ContextState, {emailAddress}): ContextState => ({ ...state, emailAddress: emailAddress }),
    [SET_REGION_CONTEXT]: (state: ContextState, {region}): ContextState => ({ ...state, region: region }),
    [SET_KEYVALUE_CONTEXT]: (state: ContextState, {keyValue}): ContextState => ({ ...state, keyValues: [...state.keyValues, keyValue] }),
    [SET_DYNAMICRESPONSE_CONTEXT]: (state: ContextState, {dynamicResponse}): ContextState => ({ ...state, dynamicResponses: [...state.dynamicResponses, dynamicResponse] }),
};

export default (state: ContextProperties = defaultContextProperties, action: ContextPropertyActions) => {
    return contextReducer[action.type] ? contextReducer[action.type](state, action) : state;
};
