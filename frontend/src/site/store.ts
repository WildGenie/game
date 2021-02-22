import { createStore, combineReducers, Store, CombinedState } from "redux"
import { currentUserReducer } from "@/site/currentUser"
import { ApplicationState } from "@/tools/definitions/general"

const state = combineReducers({
	currentUser: currentUserReducer
})

const store: Store<CombinedState<ApplicationState>> = createStore(state)

export default store