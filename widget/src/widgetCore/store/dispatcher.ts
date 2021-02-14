import { ElementType } from 'react';

import store from '.';
import * as actions from './actions';
import { LinkParams, ImageState, ContextProperties, DynamicResponse, KeyValue, KeyValues, DynamicResponses } from './types';

export function addUserMessage(text: string, id?: string) {
  store.dispatch(actions.addUserMessage(text, id));
}

export function addResponseMessage(text: string, id?: string) {
  store.dispatch(actions.addResponseMessage(text, id));
}

export function addLinkSnippet(link: LinkParams, id?: string) {
  store.dispatch(actions.addLinkSnippet(link, id));
}

export function toggleMsgLoader() {
  store.dispatch(actions.toggleMsgLoader());
}

export function renderCustomComponent(component: ElementType, props: any, showAvatar = false, id?: string) {
  store.dispatch(actions.renderCustomComponent(component, props, showAvatar, id));
}

export function toggleWidget() {
  store.dispatch(actions.toggleChat());
}

export function toggleInputDisabled() {
  store.dispatch(actions.toggleInputDisabled());
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
  store.dispatch(actions.dropMessages());
}

export function isWidgetOpened(): boolean {
  return store.getState().behavior.showChat;
}

export function setQuickButtons(buttons: Array<{ label: string, value: string | number }>) {
  store.dispatch(actions.setQuickButtons(buttons));
}

export function deleteMessages(count: number, id?: string) {
  store.dispatch(actions.deleteMessages(count, id));
}

export function markAllAsRead() {
  store.dispatch(actions.markAllMessagesRead());
}

export function setBadgeCount(count: number) {
  store.dispatch(actions.setBadgeCount(count));
}

export function openFullscreenPreview(payload: ImageState) {
  store.dispatch(actions.openFullscreenPreview(payload));
}

export function closeFullscreenPreview() {
  store.dispatch(actions.closeFullscreenPreview());
}


export function setContextProperties(contextProperties: ContextProperties) {
  store.dispatch(actions.setContextProperties(contextProperties))
}


export function setNameContext(name: string) {
  store.dispatch(actions.setNameContext(name))
}

export function setPhoneContext(phone: string) {
  store.dispatch(actions.setPhoneContext(phone))
}

export function setEmailAddressContext(emailAddress: string) {
  store.dispatch(actions.setEmailAddressContext(emailAddress))
}

export function setRegionContext(region: string) {
  store.dispatch(actions.setRegionContext(region))
}

export function addKeyValue(newKeyValue: KeyValue) {
  store.dispatch(actions.addKeyValue(newKeyValue))
}

export function addDynamicResponse(dynamicResponse: DynamicResponse) {
  store.dispatch(actions.addDynamicResponse(dynamicResponse))
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
