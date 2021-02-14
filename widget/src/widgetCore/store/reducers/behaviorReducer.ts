import { createReducer } from "../../utils/createReducer";
import { BehaviorState } from "../types";

import { BehaviorActions, DISABLE_INPUT, ENABLE_INPUT, TOGGLE_CHAT, TOGGLE_INPUT_DISABLED, TOGGLE_MESSAGE_LOADER, TOGGLE_USER_DETAILS, OPEN_USER_DETAILS, CLOSE_USER_DETAILS } from "../actions/types";

const initialState: BehaviorState = {
    showChat: false,
    disabledInput: false,
    messageLoader: false,
    userDetailsVisible: false,
};

const behaviorReducer = {
    [TOGGLE_CHAT]: (state: BehaviorState) => ({ ...state, showChat: !state.showChat }),

    [TOGGLE_INPUT_DISABLED]: (state: BehaviorState) => ({ ...state, disabledInput: !state.disabledInput }),

    [TOGGLE_MESSAGE_LOADER]: (state: BehaviorState) => ({ ...state, messageLoader: !state.messageLoader }),

    [ENABLE_INPUT]: (state: BehaviorState) => ({ ...state, disabledInput: true }),
    [DISABLE_INPUT]: (state: BehaviorState) => ({ ...state, disabledInput: false }),
    [TOGGLE_USER_DETAILS]: (state: BehaviorState) => ({ ...state, userDetailsVisible: !state.userDetailsVisible }),
    [OPEN_USER_DETAILS]: (state: BehaviorState) => ({ ...state, userDetailsVisible: true }),
    [CLOSE_USER_DETAILS]: (state: BehaviorState) => ({ ...state, userDetailsVisible: false }),
};

export default (state: BehaviorState = initialState, action: BehaviorActions) => createReducer<BehaviorState, BehaviorActions>(behaviorReducer, state, action);
