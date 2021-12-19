import { ElementType } from "react";

import * as actions from "./actions/actions";
import { LinkParams, ImageState, ContextProperties, KeyValue, DynamicResponses, KeyValues, WidgetPreferences } from "@Palavyr-Types";
import { PalavyrWidgetStore } from "./store";

export function addUserMessage(text: string, id?: string) {
    PalavyrWidgetStore.dispatch(actions._addUserMessage(text, id));
}

export function addResponseMessage(text: string, id?: string) {
    PalavyrWidgetStore.dispatch(actions._addResponseMessage(text, id));
}

export function toggleMsgLoader() {
    PalavyrWidgetStore.dispatch(actions._toggleMsgLoader());
}

export function renderCustomComponent(component: ElementType, props: any = {}, showAvatar = false, id?: string) {
    PalavyrWidgetStore.dispatch(actions._renderCustomComponent(component, props, showAvatar, id));
}

export function toggleWidget() {
    PalavyrWidgetStore.dispatch(actions._toggleChat());
}

export function toggleInputDisabled() {
    PalavyrWidgetStore.dispatch(actions._toggleInputDisabled());
}

export function openUserDetails() {
    PalavyrWidgetStore.dispatch(actions._openUserDetails());
}

export function closeUserDetails() {
    PalavyrWidgetStore.dispatch(actions._closeUserDetails());
}

export function getUserDetailsState(): boolean {
    return PalavyrWidgetStore.getState().behaviorReducer.userDetailsVisible;
}

export function toggleUserDetails() {
    PalavyrWidgetStore.dispatch(actions._toggleUserDetails());
}

export function disableInput() {
    PalavyrWidgetStore.dispatch(actions._disableInput());
}

export function enableInput() {
    PalavyrWidgetStore.dispatch(actions._enableInput());
}

export function dropMessages() {
    PalavyrWidgetStore.dispatch(actions._dropMessages());
}

export function isWidgetOpened(): boolean {
    return PalavyrWidgetStore.getState().behaviorReducer.showChat;
}

export function deleteMessages(count: number, id?: string) {
    PalavyrWidgetStore.dispatch(actions._deleteMessages(count, id));
}

export function markAllAsRead() {
    PalavyrWidgetStore.dispatch(actions._markAllMessagesRead());
}

export function setBadgeCount(count: number) {
    PalavyrWidgetStore.dispatch(actions._setBadgeCount(count));
}

export function openFullscreenPreview(payload: ImageState) {
    PalavyrWidgetStore.dispatch(actions._openFullscreenPreview(payload));
}

export function closeFullscreenPreview() {
    PalavyrWidgetStore.dispatch(actions._closeFullscreenPreview());
}

export function setWidgetPreferences(widgetPreferences: WidgetPreferences) {
    PalavyrWidgetStore.dispatch(actions._setWidgetPreferences(widgetPreferences));
}

export function setContextProperties(contextProperties: ContextProperties) {
    PalavyrWidgetStore.dispatch(actions._setContextProperties(contextProperties));
}

export function setNameContext(name: string) {
    PalavyrWidgetStore.dispatch(actions._setNameContext(name));
}

export function setPhoneContext(phone: string) {
    PalavyrWidgetStore.dispatch(actions._setPhoneContext(phone));
}

export function setEmailAddressContext(emailAddress: string) {
    PalavyrWidgetStore.dispatch(actions._setEmailAddressContext(emailAddress));
}

export function setRegionContext(region: string) {
    PalavyrWidgetStore.dispatch(actions._setRegionContext(region));
}

export function setNumIndividualsContext(numIndividuals: number) {
    PalavyrWidgetStore.dispatch(actions._setNumIndividualsContext(numIndividuals));
}

export function addKeyValue(newKeyValue: KeyValue) {
    PalavyrWidgetStore.dispatch(actions._addKeyValue(newKeyValue));
}

export function setDynamicResponses(dynamicResponseObject: DynamicResponses) {
    PalavyrWidgetStore.dispatch(actions._setDynamicResponses(dynamicResponseObject));
}

export function setPdfLink(pdfLink: string) {
    PalavyrWidgetStore.dispatch(actions._setPdfLink(pdfLink));
}


export function getContextProperties(): ContextProperties {
    const context = PalavyrWidgetStore.getState().contextReducer as ContextProperties;
    return context;
}

export function getNameContext(): string {
    return getContextProperties().name;
}

export function getPhoneContext() {
    const contextProperties = getContextProperties();
    return contextProperties.phoneNumber;
}

export function getEmailAddressContext(): string {
    const contextProperties = getContextProperties();
    return contextProperties.emailAddress;
}

export function getRegionContext(): string {
    const contextProperties = getContextProperties();
    return contextProperties.region;
}

export function getKeyValueContext(): KeyValues {
    const contextProperties = getContextProperties();
    return contextProperties.keyValues;
}

export function getDynamicResponsesContext(): DynamicResponses {
    const contextProperties = getContextProperties();
    return contextProperties.dynamicResponses;
}
