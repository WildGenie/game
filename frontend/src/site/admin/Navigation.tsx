import * as React from "react"
import { NavDestination } from "@/tools/definitions/general"
import AdminMenu from "@/components/nav/AdminMenu"

const navLinks: NavDestination[] = [
	{
		href: "/admin",
		name: "Admin home"
	},
	{
		href: "/admin/users",
		name: "Users"
	},
	{
		href: "/admin/species",
		name: "Species"
	}
]

const Navigation: React.FunctionComponent = (): JSX.Element => <AdminMenu
	links={navLinks}
	exact
/>

export default Navigation