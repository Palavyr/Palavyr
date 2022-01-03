import React, { useState, useEffect } from "react";
import { Action, ConvoNode, IPalavyrLinkedList, NodeTypeOptions, PlanTypeMeta, SetState, TreeErrors } from "@Palavyr-Types";
import { cloneDeep } from "lodash";
import { Button, makeStyles } from "@material-ui/core";
import { useParams } from "react-router-dom";
import { ConversationTreeContext, DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import PalavyrErrorBoundary from "@common/components/ErrorBoundaries/PalavyrErrorBoundary";
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
import { HeaderStrip } from "@common/components/HeaderStrip";
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
        bottom: 0,
        right: 25,
        zIndex: 99999,
    }),
    toggle: {
        position: "fixed",
        top: "70px",
        right: "15px",
        zIndex: 99999,
    },
}));

interface StructuredConvoTreeProps {
    errorCheckCallback(setTreeErrors: SetState<TreeErrors>, repository: PalavyrRepository, areaIdentifier: string, nodeList: ConvoNode[]): Promise<void>;
    historyTracker: ConversationHistoryTracker;
    nodeTypeOptions: NodeTypeOptions;
    loadNodes(): void;
    planTypeMeta: PlanTypeMeta;
    useNewEditor: boolean;
    setUseNewEditor: SetState<boolean>;
    treeErrors: TreeErrors | undefined;
    setTreeErrors: SetState<TreeErrors | undefined>;

    onSave(): void;
}

export const StructuredConvoTree = ({
    planTypeMeta,
    setUseNewEditor,
    useNewEditor,
    setTreeErrors,
    treeErrors,
    errorCheckCallback,
    historyTracker,
    loadNodes,
    nodeTypeOptions,
    onSave,
}: StructuredConvoTreeProps) => {
    const { repository, handleDrawerClose } = useContext(DashboardContext);
    const { areaIdentifier } = useParams<{ areaIdentifier: string }>();
    const [, setLoaded] = useState<boolean>(false);

    const [showDebugData, setShowDebugData] = useState<boolean>(false);
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
        const nodeList = historyTracker.linkedNodeList.compileToConvoNodes();
        if (nodeList.length > 0) {
            (async () => {
                await errorCheckCallback(setTreeErrors, repository, areaIdentifier, nodeList);
            })();
        }
        return () => {
            setTreeErrors(undefined);
        };
    }, [areaIdentifier, historyTracker.linkedNodeList]);

    const resetTree = async () => {
        const head = historyTracker.linkedNodeList.retrieveCleanHeadNode().compileConvoNode(areaIdentifier);
        const nodeTypeOptions = await repository.Conversations.GetNodeOptionsList(areaIdentifier, planTypeMeta);

        const newList = new PalavyrLinkedList([head], areaIdentifier, () => null, nodeTypeOptions, repository, []);
        historyTracker.initializeConversation(newList);
    };

    const toggleDebugData = () => {
        setShowDebugData(!showDebugData);
    };

    const getActions = () => {
        let actions: Action[] = [
            { icon: <SaveIcon />, name: "Save", onClick: onSave },
            // TODO: Major TODO!! The undo/redo buttons are not working.
            // {
            //     icon: <RedoIcon />,
            //     name: "Redo",
            //     onClick: () => historyTracker.stepConversationForwardOneStep(),
            // },
            // {
            //     icon: <UndoIcon />,
            //     name: "Undo",
            //     onClick: () => historyTracker.stepConversationBackOneStep(),
            // },
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
            const additionalActions: Action[] = [
                { icon: <BugReportIcon />, name: "Debug", onClick: toggleDebugData },
                { icon: <RotateLeftIcon />, name: "Reset Tree", onClick: async () => await resetTree() },
            ];
            actions = [...actions, ...additionalActions];
        }
        return actions;
    };
    return (
        <>
            {!useNewEditor && treeErrors && (
                <HeaderStrip
                    divider={treeErrors.anyErrors}
                    title="Chat Editor"
                    subtitle="Use this editor to create the personalized conversation flow you will provide to your potential customers. Consider planning this before implementing since you cannot modify the type of node at the beginning of the conversation without affect the nodes below."
                />
            )}
            <div>
                <ConversationTreeContext.Provider value={{ nodeTypeOptions, historyTracker, showDebugData, useNewEditor }}>
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
                        <PalavyrSpeedDial actions={getActions()} startState={true} />
                    </div>
                    <PalavyrErrorBoundary>
                        {historyTracker.linkedNodeList !== undefined &&
                            (useNewEditor ? (
                                <div className={cls.newTreeWrap}>
                                    <PalavyrFlow initialElements={cloneDeep(historyTracker.linkedNodeList.compileToNodeFlow())} />
                                </div>
                            ) : (
                                <div className={cls.treeWrap}>
                                    <ConfigurationNode currentNode={historyTracker.linkedNodeList.rootNode} pBuffer={paddingBuffer} />
                                </div>
                            ))}
                    </PalavyrErrorBoundary>
                </ConversationTreeContext.Provider>
            </div>
        </>
    );
};
