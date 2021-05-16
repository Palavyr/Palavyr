import { isDevelopmentStage } from "config/currentEnvironment"

export const logDebug = (content: any) => {
    if (isDevelopmentStage()) {
        console.log(content)
    }
}