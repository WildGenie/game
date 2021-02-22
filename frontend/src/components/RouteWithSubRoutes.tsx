import * as React from "react"
import { Route } from "react-router-dom"

type RoutePropType = {
	path: string,
	component: React.ComponentType,
	routes: Route[]
	props?: unknown
}

const RouteWithSubRoutes: React.FunctionComponent<RoutePropType> = (route: RoutePropType): JSX.Element => {
	const componentProps = route.props ? route.props : {}

	return (
		<Route
			path={route.path}
			render={props => (
				<route.component {...props} {...componentProps} routes={route.routes}/>
			)}
		/>
	)
}

export default RouteWithSubRoutes