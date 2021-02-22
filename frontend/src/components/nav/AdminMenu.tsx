import * as React from "react"
import { NavLink, useRouteMatch } from "react-router-dom"
import { NavDestination } from "@/tools/definitions/general"

type AdminMenuProps = {
	links: NavDestination[],
	backLink?: NavDestination,
	exact?: boolean
}

const AdminMenu: React.FunctionComponent<AdminMenuProps> = ({ links, backLink, exact = false }: AdminMenuProps): JSX.Element => {

	const { url } = useRouteMatch()

	return (
		<nav className="admin__nav-wrapper">
			<ul className="admin__nav">
				{backLink && (
					<li className="admin__nav-item">
						<NavLink
							className="admin__nav-link"
							to={backLink.href}
						>
							<span className="admin__back-link-icon">
								<i className="fal fa-long-arrow-left"/>
							</span>
							<span className="admin__back-link-text">
								{backLink.name || "Back"}
							</span>
						</NavLink>
					</li>
				)}
				{links.map((link, i) => {
					const href = exact ? link.href : (link.href ? `${url}/${link.href}` : url)
					return (
						<li
							className="admin__nav-item"
							key={i}
						>
							<NavLink
								className="admin__nav-link"
								to={href}
								exact={true}
							>
								{link.name}
							</NavLink>
						</li>
					)
				})}
			</ul>
		</nav>
	)
}

export default AdminMenu