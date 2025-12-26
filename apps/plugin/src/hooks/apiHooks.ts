import { pluginApiEndPoints } from "@repo/constants";
import { generateApiHooks } from "@repo/hooks";

export const apiHooks: any = generateApiHooks(pluginApiEndPoints);