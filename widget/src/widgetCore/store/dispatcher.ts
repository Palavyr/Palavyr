import { ElementType } from 'react';

import store from '.';
import * as actions from './actions';
import { LinkParams, ImageState, ContextProperties, DynamicResponse, KeyValue, KeyValues, DynamicResponses } from './types';

export function addUserMessage(text: string, id?: string) {
  store.dispatch(actions._addUserMessage(text, id));
}

export function addResponseMessage(text: string, id?: string) {
  store.dispatch(actions._addResponseMessage(text, id));
}

export function addLinkSnippet(link: LinkParams, id?: string) {
  store.dispatch(actions._addLinkSnippet(link, id));
}

export function toggleMsgLoader() {
  store.dispatch(actions._toggleMsgLoader());
}

export function renderCustomComponent(component: ElementType, props: any, showAvatar = false, id?: string) {
  store.dispatch(actions._renderCustomComponent(component, props, showAvatar, id));
}

export function toggleWidget() {
  store.dispatch(actions._toggleChat());
}

export function toggleInputDisabled() {
  store.dispatch(actions._toggleInputDisabled());
}

export function openUserDetails() {
  store.dispatch(actions._openUserDetails());
}

export function closeUserDetails() {
  store.dispatch(actions._closeUserDetails());
}

export function getUserDetailsState(): boolean {
  return store.getState().behavior.userDetailsVisible;
}


export function toggleUserDetails() {
  store.dispatch(actions._toggleUserDetails());
}

export function disableInput() {
  store.dispatch(actions._disableInput());
}

export function enableInput() {
  store.dispatch(actions._enableInput());
}

export function dropMessages() {
  store.dispatch(actions._dropMessages());
}

export function isWidgetOpened(): boolean {
  return store.getState().behavior.showChat;
}

export function setQuickButtons(buttons: Array<{ label: string, value: string | number }>) {
  store.dispatch(actions._setQuickButtons(buttons));
}

export function deleteMessages(count: number, id?: string) {
  store.dispatch(actions._deleteMessages(count, id));
}

export function markAllAsRead() {
  store.dispatch(actions._markAllMessagesRead());
}

export function setBadgeCount(count: number) {
  store.dispatch(actions._setBadgeCount(count));
}

export function openFullscreenPreview(payload: ImageState) {
  store.dispatch(actions._openFullscreenPreview(payload));
}

export function closeFullscreenPreview() {
  store.dispatch(actions._closeFullscreenPreview());
}


export function setContextProperties(contextProperties: ContextProperties) {
  store.dispatch(actions._setContextProperties(contextProperties))
}


export function setNameContext(name: string) {
  store.dispatch(actions._setNameContext(name))
}

export function setPhoneContext(phone: string) {
  store.dispatch(actions._setPhoneContext(phone))
}

export function setEmailAddressContext(emailAddress: string) {
  store.dispatch(actions._setEmailAddressContext(emailAddress))
}

export function setRegionContext(region: string) {
  store.dispatch(actions._setRegionContext(region))
}

export function addKeyValue(newKeyValue: KeyValue) {
  store.dispatch(actions._addKeyValue(newKeyValue))
}

export function addDynamicResponse(dynamicResponse: DynamicResponse) {
  store.dispatch(actions._addDynamicResponse(dynamicResponse))
}

export function getContextProperties(): ContextProperties {
  const curState = store.getState();
  const context = curState.context as ContextProperties;
  return context;
}

export function getNameContext(): string {
  const contextProperties = getContextProperties();
  return contextProperties.name;
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
