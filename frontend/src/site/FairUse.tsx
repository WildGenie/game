import * as React from "react"
import Article from "@/components/ui/articles/Article"
import ArticleHeader from "@/components/ui/articles/ArticleHeader"
import ArticleSection from "@/components/ui/articles/ArticleSection"
import ArticleParagraph from "@/components/ui/articles/ArticleParagraph"
import ArticleHeading from "@/components/ui/articles/ArticleHeading"
import { getDocumentTitle } from "@/tools/utils"

const FairUse: React.FunctionComponent = (): JSX.Element => {

	document.title = getDocumentTitle("Fair use laws")

	return (
		<div className="app__widget app__widget--lg">
			<Article>
				<ArticleHeader>
					Fair Use in the United States
				</ArticleHeader>

				<ArticleSection preamble>
					<ArticleParagraph>
						Copyrighted material has a long and rich tradition in the United States. It&apos;s even <a href="https://en.wikipedia.org/wiki/Copyright_Clause" target="_blank" rel="noreferrer">in our Constitution</a>. So how can games like this exist?
					</ArticleParagraph>
				</ArticleSection>

				<ArticleSection headerText="Introduction to Fair Use Doctrine">
					<ArticleParagraph>
						If you stop for a minute and think about it, copyrighted material is actually used all the time without the owner&apos;s permission. News broadcasts use all sorts of media, such as bits of written articles, interviews from other networks, and more. Movie reviews use clips from a film. Parody bands even use an entire song. Online retailers use trademarked icons and brand names. All of this is without fear of being sued for their use. But these are copyrighted works!
					</ArticleParagraph>

					<ArticleHeading level={3}>Enter Fair Use</ArticleHeading>

					<ArticleParagraph>
						Fair Use Doctrine is the idea that there are certain ways you can use copyrighted material without infringing on the rights of the owner, even if the owner doesn&apos;t like what you&apos;re doing. There are different types of Fair Use in US copyright law - for example, a copyrighted work (such as a book) follows Copyright Fair Use, while use of a logo or slogan follows Trademark Fair Use. Allowing Fair Use of copyrighted and trademarked material makes sense for a couple reasons.
					</ArticleParagraph>

					<ArticleParagraph>
						Firstly, a lot of &quot;fair use&quot; cases actually benefit the creator. Reviews and snippets of copyrighted work can help people decide whether they like the work itself, and can often lead to purchases. That&apos;s free advertising, which is hard to beat! Plus, the use of a logo can help buyers identify a brand quickly and generate sales. These are just two examples, but there are countless others.
					</ArticleParagraph>

					<ArticleParagraph>
						Secondly, even when fair use doesn&apos;t benefit the creator (think negative reviews, or maybe even parody), the United States has a long history of commentary and critique that&apos;s protected at the highest levels of our government and society (ever heard of the <a href="https://en.wikipedia.org/wiki/First_Amendment_to_the_United_States_Constitution" target="_blank" rel="noreferrer">First Amendment</a>?) Family Guy can make a parody of Star Wars that represents Ben Kenobi as a sick old man, and Lucasfilm can&apos;t do a thing to stop it.
					</ArticleParagraph>

					<ArticleParagraph>
						But none of these are actual legal arguments, so what <em>is</em> the legal standard for fair use?
					</ArticleParagraph>
				</ArticleSection>

				<ArticleSection headerText="Determining Fair Use">
					<ArticleParagraph>
						Fortunately, the United States has well-documented legal precedent. Bastion of Shadows deals with both copyrighted material <em>and</em> trademarked material, so we will first determine that Bastion of Shadows has a fair use from the copyright perspective - which makes proving fair use from a trademark perspective a lot easier.
					</ArticleParagraph>
				</ArticleSection>

				<ArticleSection
					headerText="Copyright and Fair Use"
					headingLevel={3}
				>
					<ArticleParagraph>
						Fair Use of copyrighted material is outlined in <a href="http://www.copyright.gov/title17/92chap1.html#107" target="_blank" rel="noreferrer">Section 107 of the Copyright Act</a>. It prescribes four criteria for determining fair use. These criteria don&apos;t need to all be met to determine fair use - fair use determination looks at the full body of Star Wars alongside the full body of Bastion of Shadows and decides whether the overall usage is fair. Here, we&apos;ll show that Bastion of Shadows strongly meets three of the four criteria and satisfies the fourth.
					</ArticleParagraph>
				</ArticleSection>
				<ArticleSection
					headerText="Purpose and character of use"
					headingLevel={4}
				>
					<ArticleParagraph>
						Essentially, the first criterion looks at how the copyrighted material is used. Is Bastion of Shadows a for-profit enterprise looking to get rich on George Lucas&apos; hard work? Or is Bastion of Shadows a totally non-profit group just trying to share in the love of Star Wars?
					</ArticleParagraph>

					<ArticleParagraph>
						Bastion of Shadows is completely free to play. That will never change. We&apos;re not trying to make any money on Star Wars. We don&apos;t offer any paid options (yet), but even when we do, those paid options will have nothing to do with Star Wars itself. Instead, paid offerings will offer in-game benefits such as additional character slots. There will be zero monetization of anything related to the Star Wars brand. The paid offerings will only be designed to cover the costs of running Bastion of Shadows, and should any profits be generated, those profits will be returned to the players in the form of lower costs of premium membership.
					</ArticleParagraph>

					<ArticleParagraph>
						Furthermore, Bastion of Shadows is a public and open source project dedicated to onboarding new software developers, designers, storytellers, and more, and offering them an opportunity to use their skills in a realistic setting that provides on-the-job experience to share with potential employers.
					</ArticleParagraph>

					<ArticleParagraph preamble>
						The purpose of Bastion of Shadows is both non-profit and educational, which both satisfy the first requirement of fair use. For this reason, we conclude that Bastion of Shadows satisfies the first requirement <em>strongly</em>.
					</ArticleParagraph>
				</ArticleSection>

				<ArticleSection
					headerText="Nature of the Copyrighted Work"
					headingLevel={4}
				>
					<ArticleParagraph>
						The second criterion looks at the copyrighted work itself. What is the purpose of the work? Is it informative or creative?
					</ArticleParagraph>

					<ArticleParagraph>
						Star Wars is a purely creative work. This does present a small problem to the legitimacy of Bastion of Shadows, but not an insurmountable one - after all, derivative and transformative works are considered fair use all the time. Such works need only fall into certain categories to be considered &quot;fair&quot;.
					</ArticleParagraph>

					<ArticleParagraph>
						Bastion of Shadows falls into several of these &quot;fair&quot; categories. Firstly and foremostly, Bastion of Shadows is <em>transformative</em>. It tells a story that is unique to Bastion of Shadows and wholly separate from the story of Star Wars. It adds new value to the idea of Star Wars by creating a new work for fans to appreciate. Secondly, Bastion of Shadows is a <em>commentary</em> on Star Wars itself, driven by fans who are free to create their characters and drive their own stories within the game however they like. Large franchises such as Star Wars often fall victim to social woes such as allegations of representational or political biases. In Bastion of Shadows and similar works, players are free to create their own commentary on the nature of Star Wars through their characters and interactions with the overall story. Players can transform Star Wars into a work that goes beyond what Star Wars itself represents, and they can use that transformation to create commentary within the Star Wars community about Star Wars itself.
					</ArticleParagraph>

					<ArticleParagraph preamble>
						The nature of Star Wars is creative, but Bastion of Shadows is as a transformative commentary. For this reason, we conclude that Bastion of Shadows satisfies the second requirement.
					</ArticleParagraph>
				</ArticleSection>

				<ArticleSection
					headerText="Amount of the copyrighted work used"
					headingLevel={4}
				>
					<ArticleParagraph>
						The third criterion looks at how much of the copyrighted work is used by Bastion of Shadows. It also looks at how substantial that portion is in relation to the overall work.
					</ArticleParagraph>

					<ArticleParagraph>
						Bastion of Shadows exists within the Star Wars universe. We can&apos;t deny that it relies heavily upon Star Wars as a work. However, Bastion of Shadows does not use any actual copyrighted work from Star Wars - no excerpts from novels, no clips from films or TV shows, and no film scores were used or ever will be used in Bastion of Shadows. Bastion of Shadows only uses the context of Star Wars as a backdrop. For example, one era of Bastion of Shadows may make reference to the Battle of Yavin because the Battle of Yavin is such a monumental event within the Star Wars franchise. However, at no point will characters be able to participate in the Battle of Yavin - the existence of the Battle of Yavin is contextually implied because of the setting of the game, but it will not be a part of the game itself. Virtually no actual copyrighted material is used in the game.
					</ArticleParagraph>

					<ArticleParagraph>
						Furthermore, as mentioned in response to criterion 2, Bastion of Shadows is a wholly transformative work. It has a story completely distinct from the story of Star Wars or its other works. The story of Bastion of Shadows will never be drawn from the story of any particular Star Wars film, novel, game, TV show, or other media. Bastion of Shadows is fully independent of Star Wars for its own existence - it merely draws inspiration and context from Star Wars. If you took Bastion of Shadows and renamed &quot;lightsabers&quot; to &quot;laser swords&quot; and changed the names of a few locations, Bastion of Shadows would be considered a similar work, but not an infringement. Its overall use of Star Wars&apos; copyrighted material is minimal.
					</ArticleParagraph>

					<ArticleParagraph preamble>
						Bastion of Shadows is a distinct work that merely draws contextual inspiration from Star Wars. Without any explicitly Star Wars-based terminology, Bastion of Shadows would be considered a unique work in its own right. For this reason, we conclude that Bastion of Shadows satisfies the third requirement <em>strongly</em>.
					</ArticleParagraph>
				</ArticleSection>

				<ArticleSection
					headerText="Effect of the use on the original work"
					headingLevel={4}
				>
					<ArticleParagraph>
						The final criterion looks at how Bastion of Shadows impacts the marketability and value of Star Wars.
					</ArticleParagraph>

					<ArticleParagraph>
						Bastion of Shadows has, at worst, a neutral impact on Star Wars&apos; market and value. Bastion of Shadows is a work of mutual appreciation among fans. It represents a place where appreciators of Star Wars can come together to create a story and experience that includes themselves. It makes Star Wars a more personal experience, and in our opinion, that can only help the Star Wars brand overall.
					</ArticleParagraph>

					<ArticleParagraph>
						Bastion of Shadows is not a competitor to any Star Wars product, because there is no Star Wars product that exists to which Bastion of Shadows is substantially similar. Even if there were a similar Star Wars product, Bastion of Shadows is a unique expression that is otherwise protected under the other three criteria of determining fair use. Bastion of Shadows only serves to improve the perception of Star Wars (and by extension, Lucasfilm and Disney) among a fanbase that is often critical and reactionary to the overall brand.
					</ArticleParagraph>

					<ArticleParagraph preamble>
						Bastion of Shadows is a non-competitor that primarily improves the subjective fan value of Star Wars overall at no direct or indirect cost to Lucasfilm or Disney. For this reason, we conclude that Bastion of Shadows satisfies the final requirement <em>strongly</em>.
					</ArticleParagraph>
				</ArticleSection>

				<ArticleSection
					headerText="Trademark and Fair Use"
					headingLevel={3}
				>
					<ArticleParagraph>
						Because we have determined that Bastion of Shadows is a fair use of Star Wars&apos; intellectual property, Bastion of Shadows is considered an independent expressive work. Since Bastion of Shadows is an independent expressive work, the <a href="https://en.wikipedia.org/wiki/Rogers_v._Grimaldi" target="_blank" rel="noreferrer">Rogers Test</a> is used to determine whether our use of Star Wars trademarks is fair. The Rogers Test (<a href="https://www.courtlistener.com/opinion/524053/ginger-rogers-v-alberto-grimaldi-mgmua-entertainment-co-and-pea/" target="_blank" rel="noreferrer">Rogers v. Grimaldi, 875 F.2d 994</a>) establishes two criteria that are used when an expressive work uses trademarked material. Under the Rogers Test, a work only needs to meet one criterion to be considered fair use, but we believe that Bastion of Shadows satisfies both.
					</ArticleParagraph>
				</ArticleSection>

				<ArticleSection
					headerText="Artistic Relevance"
					headingLevel={4}
				>
					<ArticleParagraph>
						The first criterion looks at whether the use of the trademarked material (the &quot;mark&quot;) is artistically relevant to Bastion of Shadows. Essentially, does the purpose of Bastion of Shadows justify its use of the mark?
					</ArticleParagraph>

					<ArticleParagraph>
						<i>Call of Duty: Ghosts</i> developer Activision Blizzard, Inc. was sued by Mil-Spec Monkey, Inc. in 2014. <i>COD</i> referenced Mil-Spec&apos;s famous angry monkey logo in their game, and Mil-Spec felt that this was a trademark infringement. However, the court determined that <i>COD</i> did <em>not</em> misuse the logo because the goal of <i>COD</i> is to create an authentic military-esque experience, and many active service members wear the Mil-Spec angry monkey morale patch while off-duty.
					</ArticleParagraph>

					<ArticleParagraph>
						In a similar fashion, Bastion of Shadows uses only those Star Wars-related marks that are necessary to set the stage as a legitimately transformative work. Star Wars fans have a certain expectation of works within and referencing the franchise. In pursuit of its goal of the transformation and commentary of Star Wars, Bastion of Shadows has a legitimate claim to use marks necessary to achieve that goal.
					</ArticleParagraph>

					<ArticleParagraph>
						Bastion of Shadows isn&apos;t gratuitous in its use of marks. Bastion of Shadows prohibits the use of Star Wars character names (including surnames) by players. Bastion of Shadows generally doesn&apos;t use branded Star Wars iconography, and in the few places where branded iconography does appear, it&apos;s been hand-crafted from scratch or derived from other legitimately transformative works.
					</ArticleParagraph>

					<ArticleParagraph preamble>
						Bastion of Shadows is a legitimate transformative work that makes social and other commentary on the nature and state of Star Wars. As such, certain uses of trademarks are legitimate and even necessary to meet the expectations of the community consuming Bastion of Shadows. For this reason, we conclude that Bastion of Shadows satisfies the first requirement.
					</ArticleParagraph>
				</ArticleSection>

				<ArticleSection
					headerText="Misleading about the source or content"
					headingLevel={4}
				>
					<ArticleParagraph>
						The second criterion looks at whether the use of the mark is misleading about the mark&apos;s source, ownership, or affiliation. Essentially, Bastion of Shadows can&apos;t imply, suggest, or outright claim that we own Star Wars or any of its affiliated marks (we don&apos;t). We can&apos;t claim that Lucasfilm or Disney is in a partnership with us (they aren&apos;t). Every occurrence of a Star Wars mark is used under fair use, and we explicitly attribute proper ownership of those marks on our website.
					</ArticleParagraph>

					<ArticleParagraph preamble>
						Bastion of Shadows doesn&apos;t claim ownership of Star Wars or its marks. We clearly attribute the proper ownership of those marks on our website. For that reason, we conclude that Bastion of Shadows satisfies the second requirement.
					</ArticleParagraph>
				</ArticleSection>

				<ArticleSection headerText="What does it all mean?">
					<ArticleParagraph>
						In short, it means that Bastion of Shadows securely falls into the category of fair use. Lucasfilm and Disney have a history of shutting down fan appreciation projects, but we&apos;ve demonstrated that Bastion of Shadows minimally meets all six fair use criteria that could be used to shut it down legally.
					</ArticleParagraph>
				</ArticleSection>
			</Article>
		</div>
	)
}

export default FairUse