import {
    BehaviorActions,
    BehaviorState,
    CLOSE_USER_DETAILS,
    DISABLE_INPUT,
    ENABLE_INPUT,
    OPEN_USER_DETAILS,
    TOGGLE_CHAT,
    TOGGLE_INPUT_DISABLED,
    TOGGLE_MESSAGE_LOADER,
    TOGGLE_USER_DETAILS,
} from "@Palavyr-Types";
import { createReducer } from "./createReducer";

const initialState: BehaviorState = {
    showChat: false,
    disabledInput: false,
    messageLoader: false,
    userDetailsVisible: false,
};

const reducer = {
    [TOGGLE_CHAT]: (state: BehaviorState) => ({ ...state, showChat: !state.showChat }),
    [TOGGLE_INPUT_DISABLED]: (state: BehaviorState) => ({ ...state, disabledInput: !state.disabledInput }),
    [TOGGLE_MESSAGE_LOADER]: (state: BehaviorState) => ({ ...state, messageLoader: !state.messageLoader }),
    [ENABLE_INPUT]: (state: BehaviorState) => ({ ...state, disabledInput: true }),
    [DISABLE_INPUT]: (state: BehaviorState) => ({ ...state, disabledInput: false }),
    [TOGGLE_USER_DETAILS]: (state: BehaviorState) => ({ ...state, userDetailsVisible: !state.userDetailsVisible }),
    [OPEN_USER_DETAILS]: (state: BehaviorState) => ({ ...state, userDetailsVisible: true }),
    [CLOSE_USER_DETAILS]: (state: BehaviorState) => ({ ...state, userDetailsVisible: false }),
};

export const behaviorReducer = (state: BehaviorState = initialState, action: BehaviorActions) => createReducer<BehaviorState, BehaviorActions>(reducer, state, action);
