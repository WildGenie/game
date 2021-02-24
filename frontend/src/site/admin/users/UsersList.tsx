import * as React from "react"
import { useState, useEffect, Dispatch, SetStateAction } from "react"
import { NavLink } from "react-router-dom"

import Table from "@/components/ui/tables/Table"
import TableHead from "@/components/ui/tables/TableHead"
import HeaderCell from "@/components/ui/tables/HeaderCell"
import TableRow from "@/components/ui/tables/TableRow"
import TableBody from "@/components/ui/tables/TableBody"
import BodyCell from "@/components/ui/tables/BodyCell"
import { ApplicationUserModel } from "@/tools/definitions/users"
import { getUserCount, getUsersByPage } from "@/tools/browser/admin"
import Navigation from "@/site/admin/Navigation"
import Pagination from "@/components/ui/Pagination"
import { getDocumentTitle } from "@/tools/utils"

const UsersList: React.FunctionComponent = (): JSX.Element => {

	const [resultsPerPage, setResultsPerPage] = useState(10)
	const [page, setPage] = useState(1)
	const [usersCount, setUsersCount] = useState(0)
	const [users, setUsers]: [ApplicationUserModel[], Dispatch<SetStateAction<ApplicationUserModel[]>>] = useState([])

	const updateUserCount = async () => {
		const response = await getUserCount()
		if (response.wasSuccessful)
			setUsersCount(response.result.count)
	}
	useEffect(() => {
		updateUserCount()
	}, [])

	const getUsers = async () => {
		const response = await getUsersByPage(page, resultsPerPage)
		if (!response.wasSuccessful)
			return
		setUsers(response.result)
	}
	useEffect(() => {
		getUsers()
	}, [page, resultsPerPage])

	document.title = getDocumentTitle("View users")

	return (
		<section className="admin">
			<Navigation/>

			<section className="admin__content">
				<h1>Users</h1>
				<p>There are {usersCount} registered users.</p>
				<Table>
					<TableHead>
						<TableRow>
							<HeaderCell width="8rem">Username</HeaderCell>
							<HeaderCell width="auto">Email</HeaderCell>
							<HeaderCell width="8rem">Actions</HeaderCell>
						</TableRow>
					</TableHead>

					<TableBody>
						{users.map((user, i) => (
							<TableRow key={i}>
								<BodyCell>{user.userName}</BodyCell>
								<BodyCell>{user.email}</BodyCell>
								<BodyCell>
									<NavLink
										to={`/admin/users/${user.id}`}
										title={`Edit ${user.userName}'s account`}
									>
										<i className="fal fa-user-edit"/>
									</NavLink>
								</BodyCell>
							</TableRow>
						))}
					</TableBody>
				</Table>

				<Pagination
					resultsPerPage={resultsPerPage}
					setResultsPerPage={setResultsPerPage}
					page={page}
					setPage={setPage}
					totalEntries={usersCount}
				/>
			</section>
		</section>
	)
}

export default UsersList