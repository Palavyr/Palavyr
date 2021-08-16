import React, { useCallback, useEffect, useState } from "react";
import ReactFlow from "react-flow-renderer";
import { NodeFlowInterface } from "../node/baseNode/ConfigurationNode";

export interface PalavyrFlowProps {
    elements: any;
}

const initBgColor = "#1A192B";

const connectionLineStyle = { stroke: "#fff" };
const snapGrid: [number, number] = [20, 20];
const nodeTypes = {
    selectorNode: NodeFlowInterface,
};

export const PalavyrFlow = ({ elements }: PalavyrFlowProps) => {
    const [reactflowInstance, setReactflowInstance] = useState<any>(null);

    const onLoad = useCallback(
        rfi => {
            if (!reactflowInstance) {
                setReactflowInstance(rfi);
                console.log("flow loaded:", rfi);
            }
        },
        [reactflowInstance]
    );

    useEffect(() => {
        if (reactflowInstance && elements && elements.length > 0) {
            reactflowInstance.fitView();
        }
    }, [reactflowInstance]);

    return (
        <ReactFlow
            elements={elements}
            //   onElementClick={onElementClick}
            //   onElementsRemove={onElementsRemove}
            //   onConnect={onConnect}
            // onNodeDragStop={onNodeDragStop}
            style={{ background: initBgColor }}
            onLoad={onLoad}
            nodeTypes={nodeTypes}
            connectionLineStyle={connectionLineStyle}
            snapToGrid={true}
            snapGrid={snapGrid}
            defaultZoom={1.5}
        >
            {/* <Background gap={4} size={1} color="white" /> */}
        </ReactFlow>
    );
};
