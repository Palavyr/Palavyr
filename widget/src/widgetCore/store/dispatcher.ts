import { ElementType } from 'react';

import store from '.';
import * as actions from './actions';
import { LinkParams, ImageState, ContextProperties, DynamicResponse, KeyValue } from './types';

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

// Context Property Details
// name: string;
// emailAddress: string;
// phoneNumber: string;
// region: string;
// keyValues: KeyValues;
// dynamicResponses: DynamicResponses;
export function setContextProperties(contextProperties: ContextProperties) {
  store.dispatch(actions.setContextProperties(contextProperties))
}

export function getContextProperties(): ContextProperties {
  return store.getState().context
}

export function setNameContext(name: string) {
  store.dispatch(actions.setNameContext(name))
}

export function setPhoneContext(phone: string) {
  store.dispatch(actions.setPhoneContext(phone))
}

export function setEmaillAddressContext(emailAddress: string) {
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

export function getNameContext() {
  return store.getState().context.name;
}

export function getPhoneContext() {
  return store.getState().context.phoneNumber;
}

export function getEmailAddressContext() {
  return store.getState().context.emailAddress;
}

export function getRegionContext() {
  return store.getState().context.region;
}

export function getKeyValueContext() {
  return store.getState().context.keyValues;
}

export function getDynamicResponsesContext() {
  return store.getState().context.dynamicResponses;
}
