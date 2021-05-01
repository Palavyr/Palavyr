import {
    ContextProperties,
    ContextPropertyActions,
    ContextState,
    SET_CONTEXT_PROPERTIES,
    SET_DYNAMICRESPONSES_CONTEXT,
    SET_EMAILADDRESS_CONTEXT,
    SET_KEYVALUE_CONTEXT,
    SET_NAME_CONTEXT,
    SET_NUM_INDIVIDUALS_CONTEXT,
    SET_PHONE_CONTEXT,
    SET_REGION_CONTEXT,
    SET_WIDGETPREFERENCES_CONTEXT,
} from "@Palavyr-Types";

export const defaultContextProperties: ContextProperties = {
    dynamicResponses: [],
    keyValues: [],
    emailAddress: "",
    phoneNumber: "",
    name: "",
    region: "",
    numIndividuals: null,
    widgetPreferences: null,
};

const reducer = {
    [SET_CONTEXT_PROPERTIES]: (state: ContextState, { updatedContextProperties }): ContextState => ({ ...state, ...{ contextProperties: updatedContextProperties } }),
    [SET_NUM_INDIVIDUALS_CONTEXT]: (state: ContextState, { numIndividuals }): ContextState => ({ ...state, numIndividuals: numIndividuals }),
    [SET_NAME_CONTEXT]: (state: ContextState, { name }): ContextState => ({ ...state, name: name }),
    [SET_PHONE_CONTEXT]: (state: ContextState, { phoneNumber }): ContextState => ({ ...state, phoneNumber: phoneNumber }),
    [SET_EMAILADDRESS_CONTEXT]: (state: ContextState, { emailAddress }): ContextState => ({ ...state, emailAddress: emailAddress }),
    [SET_REGION_CONTEXT]: (state: ContextState, { region }): ContextState => ({ ...state, region: region }),
    [SET_KEYVALUE_CONTEXT]: (state: ContextState, { keyValue }): ContextState => ({ ...state, keyValues: [...state.keyValues, keyValue] }),
    [SET_DYNAMICRESPONSES_CONTEXT]: (state: ContextState, { dynamicResponseObject }): ContextState => ({ ...state, dynamicResponses: dynamicResponseObject }),
    [SET_WIDGETPREFERENCES_CONTEXT]: (state: ContextState, { widgetPreferences }): ContextState => ({ ...state, widgetPreferences: widgetPreferences }),
};

export const contextReducer = (state: ContextProperties = defaultContextProperties, action: ContextPropertyActions) => {
    return reducer[action.type] ? reducer[action.type](state, action) : state;
};
