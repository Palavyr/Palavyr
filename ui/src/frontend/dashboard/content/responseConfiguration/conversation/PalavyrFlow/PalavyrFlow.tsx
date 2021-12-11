import React, { useCallback, useEffect, useState } from "react";
import ReactFlow, { Background, ConnectionLineType, Controls, isNode, MiniMap, Position, ReactFlowProvider } from "react-flow-renderer";
import { NodeFlowInterface } from "./FlowNodeInterface";
import dagre from "dagre";
import { makeStyles } from "@material-ui/core";
import { cloneDeep } from "lodash";

export interface PalavyrFlowProps {
    initialElements: any[];
}

const initBgColor = "#1A192B";

const connectionLineStyle = { stroke: "#fff" };
const snapGrid: [number, number] = [20, 20];
const nodeTypes = {
    nodeflowinterface: NodeFlowInterface,
};

const nodeWidth = 500;
const nodeHeight = 250;

const getLayoutedElements = (elements, direction = "TB") => {
    const dagreGraph = new dagre.graphlib.Graph();
    dagreGraph.setDefaultEdgeLabel(() => ({}));

    const isHorizontal = direction === "LR";
    dagreGraph.setGraph({ rankdir: direction });

    elements.forEach(el => {
        if (isNode(el)) {
            dagreGraph.setNode(el.id, { width: nodeWidth + 100, height: nodeHeight + 150 });
        } else {
            dagreGraph.setEdge(el.source, el.target);
        }
    });

    dagre.layout(dagreGraph, { ranker: "longest-path" });

    const mapped = elements.map(el => {
        if (isNode(el)) {
            const nodeWithPosition = dagreGraph.node(el.id);
            el.targetPosition = isHorizontal ? Position.Left : Position.Top;
            el.sourcePosition = isHorizontal ? Position.Right : Position.Bottom;

            // unfortunately we need this little hack to pass a slightly different position
            // to notify react flow about the change. Moreover we are shifting the dagre node position
            // (anchor=center center) to the top left so it matches the react flow node anchor point (top left).
            el.position = {
                x: nodeWithPosition.x - nodeWidth / 2 + Math.random() / 1000,
                y: nodeWithPosition.y - nodeHeight / 2,
            };
        }

        return el;
    });
    return mapped;
};

const useStyles = makeStyles(theme => ({
    minimap: { marginBottom: "6.5rem", marginRight: "2rem" },
    controls: { marginBottom: "4rem" },

}));

export const PalavyrFlow = ({ initialElements }: PalavyrFlowProps) => {
    const [reactflowInstance, setReactflowInstance] = useState<any>(null);
    const [elements, setElements] = useState<any>();
    const cls = useStyles();

    useEffect(() => {
        if (initialElements) {
            const flowElements = cloneDeep(initialElements);
            const flowWithLayout = getLayoutedElements(flowElements);
            setElements(flowWithLayout);
        }
    }, [initialElements]);

    const onLoad = useCallback(
        rfi => {
            if (!reactflowInstance) {
                setReactflowInstance(rfi);
            }
        },
        [reactflowInstance, initialElements]
    );

    useEffect(() => {
        if (reactflowInstance) {
            reactflowInstance.fitView({ paddingTop: 1.0 });
        }
    }, [reactflowInstance]);
    const initBgColor = "#1A192B";
    return elements ? (
        <ReactFlowProvider>
            <ReactFlow
                elements={elements}
                style={{ background: initBgColor }}
                onLoad={onLoad}
                nodeTypes={nodeTypes}
                connectionLineType={ConnectionLineType.Bezier}
                connectionLineStyle={connectionLineStyle}
                snapToGrid={true}
                snapGrid={snapGrid}
                defaultZoom={1}
            >
                <ConfigurationMinimap />
                <Controls className={cls.controls} />
                <Background gap={50} size={1} color="green" />
            </ReactFlow>
        </ReactFlowProvider>
    ) : (
        <></>
    );
};

export const ConfigurationMinimap = () => {
    const cls = useStyles();
    return (
        <MiniMap
            className={cls.minimap}
            nodeStrokeColor={n => {
                return "black";
            }}
            nodeColor={n => {
                if (n.type === "nodeflowinterface") return initBgColor;
                return "#fff";
            }}
        />
    );
};
