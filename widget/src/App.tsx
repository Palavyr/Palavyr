import * as React from "react";
import { useState, useCallback, useEffect } from "react";
import { CustomWidget } from "./widget/CustomWidget";
import { OptionSelector } from "./options/Options";
import { SelectedOption, WidgetPreferences } from "./types";
import { useLocation } from "react-router-dom";
import CreateClient, { IClient } from "./client/Client";

export const App = () => {
  const [selectedOption, setSelectedOption] = useState<SelectedOption | null>(
    null
  );
  const [isReady, setIsReady] = useState<boolean>(false);
  const [widgetPrefs, setWidgetPrefs] = useState<WidgetPreferences>();

  var secretKey = new URLSearchParams(useLocation().search).get("key");
  var isDemo = new URLSearchParams(useLocation().search).get("demo");

  let client: IClient;
  if (secretKey) client = CreateClient(secretKey);

  const runAppPrecheck = useCallback(async () => {
    var { data: preCheckResult } = await client.Widget.Access.runPreCheck(isDemo === "true" ? true : false);

    setIsReady(preCheckResult.isReady);
    if (preCheckResult.isReady) {
      const { data: prefs } = await client.Widget.Access.fetchPreferences();
      setWidgetPrefs(prefs);
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  useEffect(() => {
    runAppPrecheck();
  }, [runAppPrecheck]);

  return (
    <>
      {isReady === true && selectedOption === null && (
        <OptionSelector
          setSelectedOption={setSelectedOption}
          preferences={widgetPrefs}
        />
      )}
      {isReady === true && selectedOption !== null && (
        <CustomWidget option={selectedOption} preferences={widgetPrefs} />
      )}
      {isReady === false && (
        <div style={{textAlign: "center", paddingTop: "3rem"}}>
          <span>Not ready</span>
        </div>
      )}
    </>
  );
};
