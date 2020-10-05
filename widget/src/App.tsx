import React, { useState, useCallback, useEffect } from 'react';
import 'react-chat-widget/lib/styles.css';
import { CustomWidget } from './widget/CustomWidget';
import { OptionSelector } from './options/Options';
import { SelectedOption, AreaTable } from './types';
import { useParams } from 'react-router-dom';
import CreateClient from './client/Client';


type PreCheckResult = {
    isReady: boolean;
    incompleteAreas: Array<AreaTable>
}

export const App = () => {

    const [selectedOption, setSelectedOption] = useState<SelectedOption | null>(null);
    const [isReady, setIsReady] = useState<boolean>(false);

    const { secretKey } = useParams<{ secretKey: string }>();

    const client = CreateClient(secretKey);

    const runAppPrecheck = useCallback(async () => {
        var res = await client.Widget.Access.runPreCheck();
        var preCheckResult = res.data as PreCheckResult;
        setIsReady(preCheckResult.isReady);

    }, [client.Widget.Access])

    useEffect(() => {
        runAppPrecheck();
    }, [runAppPrecheck])

    return (
        <>
            {(isReady === true) && (selectedOption === null) && <OptionSelector setSelectedOption={setSelectedOption} />}
            {(isReady === true) && (selectedOption !== null) && <CustomWidget option={selectedOption} />}
            {(isReady === false) && <span style={{textAlign: "center", margin: "2rem"}}>Not ready</span>}
        </>
    )
}
