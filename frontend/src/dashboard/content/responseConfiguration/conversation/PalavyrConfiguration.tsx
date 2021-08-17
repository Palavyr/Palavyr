import React, { useState, useCallback, useEffect } from "react";
import { Action, NodeTypeOptions, TreeErrors } from "@Palavyr-Types";
import { cloneDeep } from "lodash";
import { Button, makeStyles } from "@material-ui/core";
import { useParams } from "react-router-dom";
import { ConversationTreeContext, DashboardContext } from "dashboard/layouts/DashboardContext";
import PalavyrErrorBoundary from "@common/components/Errors/PalavyrErrorBoundary";
import { ConversationHistoryTracker } from "./node/ConversationHistoryTracker";
import { PalavyrLinkedList } from "./PalavyrDataStructure/PalavyrLinkedList";
import { TreeErrorPanel } from "./MissingDynamicNodes";
import { ConfigurationNode } from "./node/baseNode/ConfigurationNode";
import { useContext } from "react";
import { PalavyrFlow } from "./PalavyrFlow/PalavyrFlow";
import $ from "jquery";
import { MAIN_CONTENT_DIV_ID, USE_NEW_EDITOR_COOKIE_NAME } from "@constants";
import { disableBodyScroll, enableBodyScroll } from "body-scroll-lock";
import Cookies from "js-cookie";
import { AreaConfigurationHeader } from "@common/components/AreaConfigurationHeader";
import { SaveOrCancel } from "@common/components/SaveOrCancel";
import UndoIcon from "@material-ui/icons/Undo";
import RedoIcon from "@material-ui/icons/Redo";
import { isDevelopmentStage } from "@api-client/clientUtils";
import AddIcon from "@material-ui/icons/Add";
import RemoveIcon from "@material-ui/icons/Remove";
import NavigateNextIcon from '@material-ui/icons/NavigateNext';

import FileCopyIcon from "@material-ui/icons/FileCopyOutlined";
import SaveIcon from "@material-ui/icons/Save";
import PrintIcon from "@material-ui/icons/Print";
import ShareIcon from "@material-ui/icons/Share";
import FavoriteIcon from "@material-ui/icons/Favorite";
import { PalavyrSpeedDial } from "@common/components/speedDial/PalavyrDial";
import BugReportIcon from '@material-ui/icons/BugReport';
import RotateLeftIcon from '@material-ui/icons/RotateLeft';

const MAIN_DIV = `#${MAIN_CONTENT_DIV_ID}`

const useStyles = makeStyles(theme => ({
    conversation: {
        position: "static",
        overflow: "auto",
    },
    treeErrorContainer: {
        margin: "0.5rem 0rem 1rem 2rem",
    },
    convoTreeMetaButtons: {
        borderRadius: "10px",
    },
    fieldSet: {
        position: "relative",
        borderWidth: "1px",
        borderStyle: "solid",
        marginBottom: "10px",
        padding: "25px",
    },

    newTreeWrap: {
        height: "93vh",
        marginTop: "-15px",
        display: "flex",
        flexDirection: "column",
        flexGrow: 1,
    },
    treeWrap: {
        position: "relative"
    },
    floatingSave: {
        position: "fixed",
        textAlign: "center",
        // borderRadius: "15px",
        // padding: "0.5rem",
        top: 200,
        // height: 500,
        right: 25,
        display: "flex",
        flexDirection: "column",
        justifyContent: "flex-end",
        alignItems: "right",
        // backgroundColor: "rgba(189, 195, 204, 0.29)",
        zIndex: 99999
    },
}));

export const StructuredConvoTree = () => {

    const cls = useStyles();
    const { planTypeMeta, repository, handleDrawerClose } = useContext(DashboardContext);
    const { areaIdentifier } = useParams<{ areaIdentifier: string }>();
    const [, setLoaded] = useState<boolean>(false);

    const [nodeTypeOptions, setNodeTypeOptions] = useState<NodeTypeOptions>([]);
    const [treeErrors, setTreeErrors] = useState<TreeErrors>();

    const [conversationHistory, setConversationHistory] = useState<PalavyrLinkedList[]>([]);
    const [conversationHistoryPosition, setConversationHistoryPosition] = useState<number>(0);
    const [showDebugData, setShowDebugData] = useState<boolean>(false);

    const [linkedNodeList, setLinkedNodes] = useState<PalavyrLinkedList>();
    const historyTracker = new ConversationHistoryTracker(setConversationHistory, setConversationHistoryPosition, setLinkedNodes);

    const [useNewEditor, setUseNewEditor] = useState<boolean>(true);
    const [paddingBuffer, setPaddingBuffer] = useState<number>(1);

    window.onbeforeunload = (e) => disableBodyScroll($(MAIN_DIV);

    const toggleUseNewEditor = () => {
        const newSettings = !useNewEditor;
        Cookies.set(USE_NEW_EDITOR_COOKIE_NAME, newSettings ? 'true' : 'false')
        setUseNewEditor(newSettings);
    }

    const setTreeWithHistory = (updatedNodeList: PalavyrLinkedList) => {
        const freshNodeList = cloneDeep(updatedNodeList);
        historyTracker.addConversationHistoryToQueue(freshNodeList, conversationHistoryPosition, conversationHistory);
        setLinkedNodes(freshNodeList);
    };

    const loadNodes = useCallback(async () => {
        if (planTypeMeta) {
            const nodes = await repository.Conversations.GetConversation(areaIdentifier);

            const nodeTypeOptions = await repository.Conversations.GetNodeOptionsList(areaIdentifier, planTypeMeta);
            const nodesLinkedList = new PalavyrLinkedList(nodes, areaIdentifier, setTreeWithHistory, nodeTypeOptions, repository);

            setNodeTypeOptions(nodeTypeOptions);
            setLinkedNodes(nodesLinkedList);

            setConversationHistory([cloneDeep(nodesLinkedList)]);
        }
    }, [areaIdentifier, planTypeMeta]);

    useEffect(() => {

        if (useNewEditor) {
            disableBodyScroll($(MAIN_DIV));
        } else {
            enableBodyScroll($(MAIN_DIV))
        }
    }, [useNewEditor])

    useEffect(() => {

        if (useNewEditor) {
            disableBodyScroll($(MAIN_DIV));
        }
        const curEditor = Cookies.get(USE_NEW_EDITOR_COOKIE_NAME)
        if (curEditor !== undefined) {
            if (curEditor === "true") {
                setUseNewEditor(true);
            } else if (curEditor === 'false') {
                setUseNewEditor(false);
            }
        }
        handleDrawerClose();
        setLoaded(true);
        loadNodes();

        return () => {

            setLoaded(false);
        };
    }, [areaIdentifier, loadNodes]);

    useEffect(() => {
        if (linkedNodeList) {
            const nodeList = linkedNodeList.compileToConvoNodes();
            if (nodeList.length > 0) {
                (async () => {
                    const treeErrors = await repository.Conversations.GetErrors(areaIdentifier, nodeList);
                    setTreeErrors(treeErrors);
                })();
            }
        }
        return () => {
            setTreeErrors(undefined);
        };
    }, [areaIdentifier, linkedNodeList]);

    const toggleDebugData = () => {
        setShowDebugData(!showDebugData);
        setLinkedNodes(cloneDeep(linkedNodeList));
    };

    const onSave = async () => {
        if (linkedNodeList && planTypeMeta) {
            const compiledNodes = linkedNodeList.compileToConvoNodes();
            const updatedConvoNodes = await repository.Conversations.ModifyConversation(compiledNodes, areaIdentifier);
            const nodeTypeOptions = await repository.Conversations.GetNodeOptionsList(areaIdentifier, planTypeMeta);
            const updatedLinkedList = new PalavyrLinkedList(updatedConvoNodes, areaIdentifier, setTreeWithHistory, nodeTypeOptions, repository);
            historyTracker.addConversationHistoryToQueue(updatedLinkedList, conversationHistoryPosition, conversationHistory);
            setLinkedNodes(updatedLinkedList);
            // window.location.reload(); // TODO: Just fix the perf problem. Clicking to many things loads too many listeners, which locks the whole browser on save.
            return true;
        } else {
            return false;
        }
    };

    const resetTree = async () => {
        if (linkedNodeList && planTypeMeta) {
            const head = linkedNodeList.retrieveCleanHeadNode().compileConvoNode(areaIdentifier);
            const nodeTypeOptions = await repository.Conversations.GetNodeOptionsList(areaIdentifier, planTypeMeta);
            const newList = new PalavyrLinkedList([head], areaIdentifier, () => null, nodeTypeOptions, repository);
            setTreeWithHistory(newList);
        }
    };



    let actions: Action[] = [
        { icon: <SaveIcon />, name: "Save", onClick: onSave},

        { icon: <UndoIcon />, name: "Undo", onClick: () => {
            historyTracker.stepConversationBackOneStep(conversationHistoryPosition, conversationHistory);
        } },
        { icon: <RedoIcon />, name: "Redo", onClick:  () => {
            historyTracker.stepConversationForwardOneStep(conversationHistoryPosition, conversationHistory);
        }},
        { icon: <RemoveIcon />, name: "Spacing -",onClick:  () => {
            if (paddingBuffer > 0.5) setPaddingBuffer(paddingBuffer - 0.5);
        }},
        { icon: <AddIcon />, name: "Spacing +", onClick: () => {
            if (paddingBuffer < 10) setPaddingBuffer(paddingBuffer + 0.5);
        } },
        { icon: <NavigateNextIcon />, name: "Switch Editor", onClick: toggleUseNewEditor },
    ];

    if (isDevelopmentStage()) {
        const additionalActions: Action[] = [
            { icon: <BugReportIcon />, name: "Debug", onClick: toggleDebugData},
            { icon: <RotateLeftIcon />, name: "Reset Tree", onClick: resetTree},

        ]
        actions =  [...actions, ...additionalActions]
    }


    return (
        <div>
            <ConversationTreeContext.Provider value={{ nodeTypeOptions, setNodes: setTreeWithHistory, conversationHistory, historyTracker, conversationHistoryPosition, showDebugData }}>
                {!useNewEditor && <AreaConfigurationHeader
                    divider={treeErrors?.anyErrors}
                    title="Chat Editor"
                    subtitle="Use this editor to create the personalized conversation flow you will provide to your potential customers. Consider planning this before implementing since you cannot modify the type of node at the beginning of the conversation without affect the nodes below."
                />}
                <div className={cls.conversation}>
                    <div className={cls.treeErrorContainer}>{treeErrors && <TreeErrorPanel treeErrors={treeErrors} />}</div>
                </div>
                <PalavyrErrorBoundary>
                    {
                        linkedNodeList !== undefined && (useNewEditor ? (
                            <div className={cls.newTreeWrap}>
                                <PalavyrFlow initialElements={linkedNodeList.compileToNodeFlow()} />
                            </div>
                        ) : (
                            <div className={cls.treeWrap}>
                                <ConfigurationNode currentNode={linkedNodeList.rootNode} pBuffer={paddingBuffer} />
                            </div>

                        )
                        )
                    }

                </PalavyrErrorBoundary>
                <div className={cls.floatingSave}>
                    <PalavyrSpeedDial actions={actions} />
                </div>
            </ConversationTreeContext.Provider>
        </div>
    );
};



