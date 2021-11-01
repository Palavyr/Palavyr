import React, { useState, useCallback, useEffect } from "react";
import { Action, ConvoNode, NodeTypeOptions, SetState, TreeErrors } from "@Palavyr-Types";
import { cloneDeep } from "lodash";
import { Button, makeStyles } from "@material-ui/core";
import { useParams } from "react-router-dom";
import { ConversationTreeContext, DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
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
import UndoIcon from "@material-ui/icons/Undo";
import RedoIcon from "@material-ui/icons/Redo";
import { isDevelopmentStage } from "@common/client/clientUtils";
import AddIcon from "@material-ui/icons/Add";
import RemoveIcon from "@material-ui/icons/Remove";
import SaveIcon from "@material-ui/icons/Save";
import { PalavyrSpeedDial } from "@common/components/speedDial/PalavyrDial";
import BugReportIcon from "@material-ui/icons/BugReport";
import RotateLeftIcon from "@material-ui/icons/RotateLeft";
import { PalavyrRepository } from "@common/client/PalavyrRepository";

const MAIN_DIV = `#${MAIN_CONTENT_DIV_ID}`;

type StyleProps = {
    useNewEditor: boolean;
};
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
        position: "relative",
    },
    treeWrap: {
        position: "relative",
    },
    floatingSave: (props: StyleProps) => ({
        position: "fixed",
        bottom: props.useNewEditor ? 190 : 50,
        right: 10,
        zIndex: 99999,
    }),
    toggle: {
        position: "fixed",
        top: "120px",
        right: "15px",
        zIndex: 99999,
    },
}));

// if (nodeList.length > 0) {
//     (async () => {
//         const treeErrors = await repository.Conversations.GetErrors(areaIdentifier, nodeList);
//         setTreeErrors(treeErrors);
//     })();
// }

export const ConversationConfigurationPage = () => {
    const errorCheckCallback = async (setTreeErrors: SetState<TreeErrors>, repository: PalavyrRepository, areaIdentifier: string, nodeList: ConvoNode[]) => {
        const treeErrors = await repository.Conversations.GetErrors(areaIdentifier, nodeList);
        setTreeErrors(treeErrors);
    };
    return <StructuredConvoTree isIntroduction={false} errorCheckCallback={errorCheckCallback} />;
};

export const IntroConversationConfigurationPage = () => {
    const errorCheckCallback = async (setTreeErrors: SetState<TreeErrors>, repository: PalavyrRepository, areaIdentifier: string, nodeList: ConvoNode[]) => {
        const treeErrors = await repository.Conversations.GetIntroErrors(areaIdentifier, nodeList);
        setTreeErrors(treeErrors);
    };
    return <StructuredConvoTree isIntroduction={true} errorCheckCallback={errorCheckCallback} />;
};

interface StructuredConvoTreeProps {
    isIntroduction: boolean;
    errorCheckCallback(setTreeErrors: SetState<TreeErrors>, repository: PalavyrRepository, areaIdentifier: string, nodeList: ConvoNode[]): Promise<void>;
}
export const StructuredConvoTree = ({ isIntroduction = false, errorCheckCallback }: StructuredConvoTreeProps) => {
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

    const cls = useStyles({ useNewEditor });

    window.onbeforeunload = e => enableBodyScroll($(MAIN_DIV));

    const toggleUseNewEditor = () => {

        if (useNewEditor) {
            enableBodyScroll($`#${MAIN_CONTENT_DIV_ID}`);
        } else {
            disableBodyScroll($`#${MAIN_CONTENT_DIV_ID}`);
        }

        const newSettings = !useNewEditor;
        Cookies.set(USE_NEW_EDITOR_COOKIE_NAME, newSettings ? "true" : "false");
        setUseNewEditor(newSettings);
    };

    const setTreeWithHistory = (updatedNodeList: PalavyrLinkedList) => {
        const freshNodeList = cloneDeep(updatedNodeList);
        historyTracker.addConversationHistoryToQueue(freshNodeList, conversationHistoryPosition, conversationHistory);
        setLinkedNodes(freshNodeList);
    };

    const loadNodes = useCallback(async () => {
        if (planTypeMeta) {
            const nodes = await repository.Conversations.GetConversation(areaIdentifier);

            let nodeTypeOptions: NodeTypeOptions;
            if (isIntroduction) {
                nodeTypeOptions = await repository.Conversations.GetIntroNodeOptionsList();
            } else {
                nodeTypeOptions = await repository.Conversations.GetNodeOptionsList(areaIdentifier, planTypeMeta);
            }

            const nodesLinkedList = new PalavyrLinkedList(nodes, areaIdentifier, setTreeWithHistory, nodeTypeOptions, repository);
            setNodeTypeOptions(nodeTypeOptions);
            setLinkedNodes(nodesLinkedList);

            setConversationHistory([cloneDeep(nodesLinkedList)]);
        }
    }, [areaIdentifier, planTypeMeta]);

    useEffect(() => {
        if (useNewEditor) {
            window.scrollTo(0, 0);
        } else {
            enableBodyScroll($(MAIN_DIV));
        }
    }, [useNewEditor]);

    useEffect(() => {
        const curEditor = Cookies.get(USE_NEW_EDITOR_COOKIE_NAME);
        if (curEditor !== undefined) {
            if (curEditor === "true") {
                setUseNewEditor(true);
            } else if (curEditor === "false") {
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
                    await errorCheckCallback(setTreeErrors, repository, areaIdentifier, nodeList);
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
        if (isIntroduction) {
            if (linkedNodeList && planTypeMeta) {
                const compiledNodes = linkedNodeList.compileToConvoNodes();
                const updatedConvoNodes = await repository.Settings.Account.updateIntroduction(areaIdentifier, compiledNodes);
                let nodeTypeOptions = await repository.Conversations.GetNodeOptionsList(areaIdentifier, planTypeMeta);
                nodeTypeOptions = nodeTypeOptions.filter(x => x.value === "ProvideInfo");

                const updatedLinkedList = new PalavyrLinkedList(updatedConvoNodes, areaIdentifier, setTreeWithHistory, nodeTypeOptions, repository);
                historyTracker.addConversationHistoryToQueue(updatedLinkedList, conversationHistoryPosition, conversationHistory);
                setLinkedNodes(updatedLinkedList);
                // window.location.reload(); // TODO: Just fix the perf problem. Clicking to many things loads too many listeners, which locks the whole browser on save.
                return true;
            } else {
                return false;
            }
        } else {
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

    const stepBack = () => {
        historyTracker.stepConversationBackOneStep(conversationHistoryPosition, conversationHistory);
    };

    const stepForward = () => {
        historyTracker.stepConversationForwardOneStep(conversationHistoryPosition, conversationHistory);
    };

    let actions: Action[] = [
        { icon: <SaveIcon />, name: "Save", onClick: onSave },
        {
            icon: <RedoIcon />,
            name: "Redo",
            onClick: stepForward,
        },
        {
            icon: <UndoIcon />,
            name: "Undo",
            onClick: stepBack,
        },
    ];

    if (!useNewEditor) {
        const additionalEditorActions: Action[] = [
            {
                icon: <RemoveIcon />,
                name: "Spacing -",
                onClick: () => {
                    if (paddingBuffer > 0.5) setPaddingBuffer(paddingBuffer - 0.5);
                },
            },
            {
                icon: <AddIcon />,
                name: "Spacing +",
                onClick: () => {
                    if (paddingBuffer < 10) setPaddingBuffer(paddingBuffer + 0.5);
                },
            },
        ];
        actions = [...actions, ...additionalEditorActions];
    }

    if (isDevelopmentStage()) {
        const additionalActions: Action[] = [{ icon: <BugReportIcon />, name: "Debug", onClick: toggleDebugData }, { icon: <RotateLeftIcon />, name: "Reset Tree", onClick: resetTree }];
        actions = [...actions, ...additionalActions];
    }

    return (
        <div>
            <ConversationTreeContext.Provider value={{ nodeTypeOptions, setNodes: setTreeWithHistory, conversationHistory, historyTracker, conversationHistoryPosition, showDebugData, useNewEditor }}>
                {!useNewEditor && treeErrors && (
                    <AreaConfigurationHeader
                        divider={treeErrors.anyErrors}
                        title="Chat Editor"
                        subtitle="Use this editor to create the personalized conversation flow you will provide to your potential customers. Consider planning this before implementing since you cannot modify the type of node at the beginning of the conversation without affect the nodes below."
                    />
                )}
                <div className={cls.conversation}>
                    <div className={cls.treeErrorContainer}>{treeErrors && <TreeErrorPanel treeErrors={treeErrors} />}</div>
                </div>
                <div className={cls.toggle}>
                    {useNewEditor ? (
                        <Button size="small" variant="contained" color="primary" onClick={toggleUseNewEditor}>
                            Use Classic Editor
                        </Button>
                    ) : (
                        <Button size="small" variant="contained" color="primary" onClick={toggleUseNewEditor}>
                            Use Cool New Editor
                        </Button>
                    )}
                </div>
                <div className={cls.floatingSave}>
                    <PalavyrSpeedDial actions={actions} />
                </div>
                <PalavyrErrorBoundary>
                    {linkedNodeList !== undefined &&
                        (useNewEditor ? (
                            <div className={cls.newTreeWrap}>
                                <PalavyrFlow initialElements={cloneDeep(linkedNodeList.compileToNodeFlow())} />
                            </div>
                        ) : (
                            <div className={cls.treeWrap}>
                                <ConfigurationNode currentNode={linkedNodeList.rootNode} pBuffer={paddingBuffer} />
                            </div>
                        ))}
                </PalavyrErrorBoundary>
            </ConversationTreeContext.Provider>
        </div>
    );
};
