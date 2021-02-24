import * as React from "react"
import { useState, useRef, useEffect, SetStateAction, Dispatch } from "react"
import { NavLink } from "react-router-dom"
import { useSelector } from "react-redux"

import "./MainMenu.scss"
import Logo from "./logo.png"

import { getCurrentUser } from "@/site/currentUser"
import { Roles } from "@/tools/definitions/general"

const MainMenu: React.FunctionComponent = (): JSX.Element => {
	const { isLoggedIn, username, roles }: { isLoggedIn: boolean, username: string, roles: string[]} = useSelector(getCurrentUser)
	const isAdmin = roles && roles.includes(Roles.administrator)

	const [open, setOpen]: [boolean, Dispatch<SetStateAction<boolean>>] = useState(false)

	const ref: React.MutableRefObject<HTMLElement> = useRef()

	useEffect(() => {
		if (!open)
			return

		const handleDocumentClick = event => {
			if (ref.current && ref.current.contains(event.target))
				return

			setOpen(!open)
		}

		document.addEventListener("click", handleDocumentClick, { capture: true })

		return () => document.removeEventListener("click", handleDocumentClick, { capture: true })
	}, [open])

	const links = [
		{
			href: "/",
			text: "Home"
		},
		{
			href: isLoggedIn ? "/account" : "",
			text: isLoggedIn ? username : ""
		},
		{
			href: isLoggedIn ? "/users/logout" : "/users/login",
			text: isLoggedIn ? "Log Out" : "Log In"
		}
	]

	return (
		<nav
			ref={ref}
			className="app__nav main-menu"
		>
			<div className="main-menu__mobile-wrapper">
				<div className="main-menu__logo-wrapper">
					<NavLink to="/">
						<img
							className="main-menu__logo"
							src={Logo}
							alt="Company logo"
						/>
					</NavLink>
				</div>

				<button
					className="main-menu__toggler"
					onClick={() => setOpen(!open)}
				>
					<i className={`far ${open ? "fa-times" : "fa-bars"}`}/>
				</button>
			</div>

			<ul className={`main-menu__menu ${open ? "main-menu__menu--open" : ""}`}>
				{links.map(link => {
					if (!link.href)
						return null

					return (
						<li
							key={link.href}
							className="main-menu__menu-item"
							onClick={() => setOpen(false)}
						>
							<NavLink
								className="main-menu__link"
								to={link.href}
							>{link.text}</NavLink>
						</li>
					)
				})}
				{isAdmin && (
					<li
						className="main-menu__menu-item"
						onClick={() => setOpen(!open)}
					>
						<NavLink
							className="main-menu__link"
							to="/admin"
						>
							Admin
						</NavLink>
					</li>
				)}
			</ul>
		</nav>
	)
}

export default MainMenu